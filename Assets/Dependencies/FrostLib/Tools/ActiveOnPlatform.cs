using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrostLib.Tools
{
    [AddComponentMenu("GoDice/App/[FrostLib.Tools] Active On Platform")]
    internal class ActiveOnPlatform : MonoBehaviour
    {
        [Flags]
        private enum Platform
        {
            None = 0,
            Editor = 1,
            Android = 2,
            iOS = 4,
            Windows = 8,
            MacOS = 16
        }

        [SerializeField] private Platform _platforms;

        public void Awake()
        {
            var flag =
#if UNITY_IOS
                Platform.iOS;
#elif UNITY_EDITOR
                Platform.Editor;
#elif UNITY_ANDROID
                Platform.Android;
#elif UNITY_STANDALONE_WIN
                Platform.Windows;
#elif UNITY_STANDALONE_OSX
                Platform.MacOS;
#else
                Platform.None;
#endif
            var isActive = flag != Platform.None && _platforms.HasFlag(flag);
            gameObject.SetActive(isActive);
        }
    }
}