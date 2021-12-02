using FrostLib.Services;
using GoDice.Shared.Controls;
using GoDice.Shared.EventDispatching.Dispatching;
using GoDice.Shared.EventDispatching.Events;
using UnityEngine;

namespace GoDice.Shared.Components
{
    public abstract class EventOnClickBase : MonoBehaviour
    { 
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Servicer.Get<IEventDispatcher>();

        private InteractableProxy _interactable;

        protected virtual void Awake()
        {
            _interactable = new InteractableProxy(gameObject);
            _interactable.OnClick.AddListener(TryRaiseEvent);
        }

        protected abstract void TryRaiseEvent();
        
        protected static void Raise(IEvent ev) => Dispatcher.Raise(ev);

        protected void SetInteractable(bool isOn) => _interactable.SetInteractable(isOn);

        private void OnDestroy() => _interactable?.Dispose();
    }
}