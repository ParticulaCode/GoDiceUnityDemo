using System;
using GoDice.App.Modules.Dice.Messaging;
using GoDice.Shared.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class ColorModule : Module
    {
        [HideLabel]
        [SerializeField]
        [EnumToggleButtons]
        [OnValueChanged(nameof(Send))]
        private ColorType _color = ColorType.None;

        public void Send() =>
            SendData(Reader.BuildResponse(Response.Color, new[] { (byte) (_color - 1) }));
    }
}