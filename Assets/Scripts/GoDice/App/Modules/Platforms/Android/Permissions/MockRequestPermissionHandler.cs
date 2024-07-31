using GoDice.Shared.EventDispatching.Handlers;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Platforms.Android.Permissions
{
    [UsedImplicitly]
    internal class MockRequestPermissionHandler : EventHandler
    {
        public MockRequestPermissionHandler(RequestPermissionEvent ev) : base(ev)
        {
        }

        public override void Handle() => EventAs<RequestPermissionEvent>().Ok(true);
    }
}