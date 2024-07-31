using GoDice.App.Modules.Platforms.Android.Permissions;
using GoDice.Shared.EventDispatching.Binding;
using GoDice.Shared.EventDispatching.Dispatching;
using UnityEngine;
using static GoDice.Shared.EventDispatching.Events.EventType;

namespace GoDice.App.Modules.Platforms
{
    public class Bootstrapper
    {
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Load(IEventDispatcher dispatcher) => BindPermissionsHandler(dispatcher);

        private static void BindPermissionsHandler(IEventDispatcher dispatcher)
        {
            if (Application.isEditor)
            {
                dispatcher.Bind().Handler<MockRequestPermissionHandler>().To(RequestAndroidPermission);
                return;
            }

#if UNITY_ANDROID
            dispatcher.Bind().Handler<RequestPermissionHandler>().To(RequestAndroidPermission);
#else
            dispatcher.Bind().Handler<MockRequestPermissionHandler>().To(RequestAndroidPermission);
#endif
        }
    }
}