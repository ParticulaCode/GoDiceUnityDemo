using System;
using GoDice.App.Modules.Dice.Messaging;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal abstract class TapModuleBase : Module
    {
        [ReadOnly]
        [SerializeField]
        [HorizontalGroup("Group", LabelWidth = 110, Width = 0.3f)]
        private bool _interruptOpened;

        protected abstract Response Response { get; }

        [HorizontalGroup("Group")]
        [GUIColor(0, 1f, 0)]
        [EnableIf(nameof(_interruptOpened))]
        [Button("Send event", ButtonSizes.Large)]
        public void Tap() => SendData(Reader.BuildResponse(Response));

        public void OpenInterrupt() => _interruptOpened = true;

        public void CloseInterrupt() => _interruptOpened = false;
    }
}