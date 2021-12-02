using System.Collections.Generic;
using FrostLib.Services;
using GoDice.App.Modules.Dice.Core;
using UnityEngine;

namespace GoDice.App.Modules.Dice.Presentation
{
    public abstract class DicePresenterBase : MonoBehaviour, IConnectedDicePresenter
    {
        protected static ServiceLocator Locator => ServiceLocator.Instance;
        private static IDiceHolder Holder => Locator.Get<IDiceHolder>();
        private static IConnectedDicePresentersManager Presentator => Locator.Get<IConnectedDicePresentersManager>();

        private void Start()
        {
            Presentator.Add(this);
            Present(Holder.GetConnectedDice());
        }

        public abstract void Present(IEnumerable<IDie> dice);

        protected virtual void OnDestroy() => Presentator.Remove(this);
    }
}