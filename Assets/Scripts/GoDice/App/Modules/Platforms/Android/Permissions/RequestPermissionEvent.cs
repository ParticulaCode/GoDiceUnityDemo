using System;
using GoDice.Shared.EventDispatching.Events;

namespace GoDice.App.Modules.Platforms.Android.Permissions
{
    public class RequestPermissionEvent : RoundTripEvent<bool>
    {
        public override EventType Type => EventType.RequestAndroidPermission;
        public readonly string Permission;

        public RequestPermissionEvent(string permission, Action<bool> cb) : base(cb) => Permission = permission;
    }
}