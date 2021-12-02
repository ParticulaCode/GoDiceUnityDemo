using System;
using GoDice.App.Modules.Dice.Messaging;
using UnityEngine;

namespace GoDice.App.Modules.Simulation.Modules
{
    [Serializable]
    internal class Module : MonoBehaviour
    {
        protected readonly Reader Reader = new Reader();

        private Action<byte[]> _dataSender;

        public virtual void Initialize(Action<byte[]> dataSender) => _dataSender = dataSender;

        protected void SendData(byte[] bytes) => _dataSender?.Invoke(bytes);
    }
}