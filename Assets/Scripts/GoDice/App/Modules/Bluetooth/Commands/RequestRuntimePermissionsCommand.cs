using System.Text.RegularExpressions;
using System.Threading;
using Cysharp.Threading.Tasks;
using FrostLib.Commands.Async;
using FrostLib.Services;
using GoDice.App.Modules.Platforms.Android.Permissions;
using GoDice.Shared.EventDispatching.Dispatching;
using GoDice.Shared.EventDispatching.Exceptions;
using JetBrains.Annotations;
using UnityEngine;

namespace GoDice.App.Modules.Bluetooth.Commands
{
    [UsedImplicitly]
    internal class RequestRuntimePermissionsCommand : ITaskCommand
    {
        private static ServiceLocator Locator => ServiceLocator.Instance;
        private static IEventDispatcher Dispatcher => Locator.Get<IEventDispatcher>();

        private static readonly string[] Permissions =
        {
            "android.permission.BLUETOOTH_CONNECT",
            "android.permission.BLUETOOTH_SCAN"
        };

        public async UniTask Execute(CancellationToken cancellationToken = default)
        {
            //These two permissions required only starting from Android 12
            if (Application.isEditor || GetApiLevel() < 31)
                return;

            foreach (var permission in Permissions)
            {
                var isPermissionGranted = false;
                var gotResponse = false;
                Dispatcher.Raise(new RequestPermissionEvent(permission, response =>
                {
                    isPermissionGranted = response;
                    gotResponse = true;
                }));

                await UniTask.WaitUntil(() => gotResponse, cancellationToken: cancellationToken);

                if (!isPermissionGranted)
                    throw new SequenceCanceledException($"Permission '{permission}' is not granted.");
            }
        }

        private static int GetApiLevel()
        {
            const int fallbackLevel = int.MaxValue;
            try
            {
                //Android OS 12 / API-31 (SP1A.210812.016/T500XXU3CVE7)
                var match = Regex.Match(SystemInfo.operatingSystem, @"API-(\d+)");
                return match.Groups.Count < 2 ? fallbackLevel : int.Parse(match.Groups[1].Value);
            }
            catch
            {
                return fallbackLevel;
            }
        }
    }
}