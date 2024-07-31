using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using FrostLib.Coroutines;
using FrostLib.Extensions;
using FrostLib.Signals.impl;
using FrostLib.Tasks;
using GoDice.Shared.EventDispatching.Binding;
using GoDice.Shared.EventDispatching.Events;
using GoDice.Shared.EventDispatching.Exceptions;
using GoDice.Shared.EventDispatching.Handlers;
using MoreLinq;
using UnityEngine;

namespace GoDice.Shared.EventDispatching.Dispatching
{
    public class EventDispatcher : IEventDispatcher
    {
        public Signal<EventHandlerBase, Events.EventType> OnHandlerCreatedSignal { get; } = new();

        public Signal<Events.EventType> OnRaisingEventSignal { get; } = new();

        public Signal<Exception, ExceptionType> OnCaughtExceptionSignal { get; } = new();

        public Signal<CancellationExceptionInfo> OnCancellationExceptionSignal { get; } = new();

        private readonly IDictionary<Events.EventType, List<Group>> _handlers =
            Enum<Events.EventType>.GetEnumValues()
                .ToDictionary(evType => evType, _ => new List<Group>());

        private readonly RoutineRunner _routiner;
        private readonly ICancellationTokenFactory _tokenFactory;

        public EventDispatcher(RoutineRunner routiner, ICancellationTokenFactory tokenFactory)
        {
            _routiner = routiner;
            _tokenFactory = tokenFactory;
        }

        Binder<EventHandlerBase> IEventDispatcher.Bind() => CreateBinder();

        private Binder<EventHandlerBase> CreateBinder() => new(AddHandlersGroup);

        private void AddHandlersGroup(Group group, Events.EventType eventType) =>
            _handlers[eventType].AddDistinct(group);

        public IUnbinder Unbind<T>() where T : EventHandlerBase =>
            new SingleUnbinder<T>(RemoveSingleHandler);

        public IUnbinder UnbindSequenceWith<T>() where T : EventHandlerBase =>
            new SingleUnbinder<T>(RemoveSequenceContaining);

        private void RemoveSingleHandler(Type type, Events.EventType eventType) =>
            _handlers[eventType].RemoveAny(group => group.Handlers.Count() == 1
                                                    && group.Handlers.First() == type);

        private void RemoveSequenceContaining(Type type, Events.EventType eventType) =>
            _handlers[eventType].RemoveAny(group => group.IsSequential
                                                    && group.Handlers.Contains(type));

        public IUnbinder Unbind(Group group) => new GroupUnbinder(group, RemoveHandlersGroup);

        private void RemoveHandlersGroup(Group group, Events.EventType eventType) =>
            _handlers[eventType].Remove(group);

        void IEventDispatcher.Raise(IEvent ev)
        {
            var groups = _handlers[ev.Type].ToArray();
            if (groups.Length == 0)
            {
                if (Debug.isDebugBuild)
                    Debug.LogWarning($"Handler is missing for the event: {ev.Type}.");

                return;
            }

            OnRaisingEventSignal.Dispatch(ev.Type);

            Process(ev, groups);
        }

        private void Process(IEvent ev, IEnumerable<Group> groups)
        {
            foreach (var group in groups)
            {
                if (group.UnbindAfterFirstExecution)
                    Unbind(group).From(ev.Type);

                if (group.IsSequential)
                    ProcessSequence(ev, group);
                else
                    group.Handlers.ForEach(handlerType =>
                        ProcessEvent(ev, handlerType, group));
            }
        }

        private async void ProcessSequence(IEvent ev, Group group)
        {
            var (token, onTaskFinished) = GetToken(group.CancelOnSceneSwitch);

            try
            {
                foreach (var handlerType in group.Handlers)
                    await ProcessEventAsync(ev, handlerType, token);
            }
            catch (OperationCanceledException e)
            {
                OnCancellationExceptionSignal.Dispatch(
                    new CancellationExceptionInfo(e, group, ev.Type));
            }
            finally
            {
                if (group.DisposeSceneCancellationHookAfterHandling)
                    onTaskFinished?.Invoke();
            }
        }

        private void ProcessEvent(IEvent ev, Type handlerType, Group group)
        {
            var handlerBase = CreateHandler(ev, handlerType);
            switch (handlerBase)
            {
                case IHandler handler:
                {
                    handler.Handle();
                    break;
                }
                case IRoutinedHandler routinedHandler:
                {
                    _routiner.StartRoutine(routinedHandler.Handle(), group.CancelOnSceneSwitch);
                    break;
                }
                case ITaskHandler taskHandler:
                {
                    RunAndForget(taskHandler, group);
                    break;
                }
            }
        }

        //If task is not awaited, then any internal exception will be hidden
        private async void RunAndForget(ITaskHandler taskHandler, Group group)
        {
            var (token, onTaskFinished) = GetToken(group.CancelOnSceneSwitch);
            try
            {
                await taskHandler.Handle(token);
            }
            catch (OperationCanceledException e)
            {
                OnCaughtExceptionSignal.Dispatch(e, ExceptionType.Operation);
            }
            catch (SequenceCanceledException e)
            {
                OnCaughtExceptionSignal.Dispatch(e, ExceptionType.Sequence);
            }
            finally
            {
                if (group.DisposeSceneCancellationHookAfterHandling)
                    onTaskFinished?.Invoke();
            }
        }

        private async UniTask ProcessEventAsync(IEvent ev, Type handlerType,
            CancellationToken cancellationToken)
        {
            Coroutine coroutine = null;
            try
            {
                var handlerBase = CreateHandler(ev, handlerType);
                switch (handlerBase)
                {
                    case IHandler handler:
                    {
                        handler.Handle();
                        break;
                    }
                    case IRoutinedHandler routinedHandler:
                    {
                        //1) UniTask requires MonoBehaviour provided to await anything else that is not:
                        //CustomYieldInstruction, AsyncOperation or WaitForSeconds
                        //Otherwise warning: "$"yield {current.GetType().Name} is not supported on await IEnumerator or IEnumerator.ToUniTask()"
                        //2) UniTask will cancel task, but not coroutine if cancellation is requested. We have to do it manually
                        var source = AutoResetUniTaskCompletionSource.Create();
                        var coroutineWithPromise =
                            WrapCoroutineWithPromise(routinedHandler.Handle(), source);
                        coroutine = _routiner.StartRoutine(coroutineWithPromise, false);
                        await source.Task.AttachExternalCancellation(cancellationToken);
                        break;
                    }
                    case ITaskHandler taskHandler:
                    {
                        await taskHandler.Handle(cancellationToken);
                        break;
                    }
                }
            }
            catch (OperationCanceledException e)
            {
                OnCaughtExceptionSignal.Dispatch(e, ExceptionType.OperationAsync);

                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException();
            }
            catch (SequenceCanceledException e)
            {
                OnCaughtExceptionSignal.Dispatch(e, ExceptionType.SequenceAsync);

                throw new OperationCanceledException();
            }
            finally
            {
                if (coroutine != null)
                    _routiner.StopRoutine(coroutine);
            }
        }

        private static IEnumerator WrapCoroutineWithPromise(IEnumerator inner, IResolvePromise source)
        {
            yield return inner;

            source.TrySetResult();
        }

        private EventHandlerBase CreateHandler(IEvent ev, Type handlerType)
        {
            var handler = (EventHandlerBase) Activator.CreateInstance(handlerType, ev);
            var injector = new ServicesInjector<EventHandlerBase>();
            injector.Inject(handler);

            OnHandlerCreatedSignal.Dispatch(handler, ev.Type);

            return handler;
        }

        private (CancellationToken Token, Action OnTaskFinished) GetToken(bool cancelOnSceneSwitch) =>
            cancelOnSceneSwitch
                ? _tokenFactory.GetSceneSwitchedToken()
                : (_tokenFactory.GetAppClosingToken(), null);
    }
}