#if UNITY_ANDROID
using System.Threading;
using Cysharp.Threading.Tasks;
using GoDice.Shared.EventDispatching.Handlers;
using JetBrains.Annotations;
using Plugins.Android.PermissionsManager;

namespace GoDice.App.Modules.Platforms.Android.Permissions
{
    [UsedImplicitly]
    internal class RequestPermissionHandler : TaskEventHandler
    {
        private enum Status
        {
            Pending,
            Granted,
            Denied,
            DeniedAndDontAsk
        }

        private readonly AndroidPermissionCallback _callbacks;
        private Status _status;

        public RequestPermissionHandler(RequestPermissionEvent ev) : base(ev) =>
            _callbacks = new AndroidPermissionCallback(
                OnPermissionGranted,
                OnPermissionDenied,
                OnPermissionDeniedAndDontAsk);

        public override async UniTask Handle(CancellationToken cancellationToken = default)
        {
            var ev = EventAs<RequestPermissionEvent>();
            if (AndroidPermissionsManager.IsPermissionGranted(ev.Permission))
            {
                ev.Ok(true);
                return;
            }

            SetStatus(Status.Pending);
            RequestPermission(ev.Permission);

            await UniTask.WaitWhile(() => _status == Status.Pending,
                cancellationToken: cancellationToken);

            Log.Message($"Permission response: {_status} [{ev.Permission}]");

            ev.Ok(_status == Status.Granted);
        }

        private void SetStatus(Status newStatus) => _status = newStatus;

        private void RequestPermission(string permission) =>
            AndroidPermissionsManager.RequestPermission(permission, _callbacks);

        //Called not from the main thread
        private void OnPermissionGranted(string permission) => SetStatus(Status.Granted);

        //Called not from the main thread
        private void OnPermissionDenied(string permission) => SetStatus(Status.Denied);

        //Called not from the main thread
        private void OnPermissionDeniedAndDontAsk(string permission) =>
            SetStatus(Status.DeniedAndDontAsk);
    }
}
#endif