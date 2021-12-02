using System;
using System.Collections.Generic;
using System.Linq;
using FrostLib.Extensions;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Views;
using MoreLinq;
using UnityEngine;
using UnityEngine.UI;

namespace GoDice.App.Modules.Dice.Presentation
{
    [AddComponentMenu("GoDice/App/Dice/Connected Dice Presenter")]
    internal class ViewPresenter : DicePresenterBase
    {
        [SerializeField] private Transform _container;
        [SerializeField] private Die2dView _prefab;

        private readonly List<Die2dView> _views = new List<Die2dView>(6);

        public override void Present(IEnumerable<IDie> dice)
        {
            var relevantDice = dice.OfType<Die>().ToArray();
            var presentedDice = _views.Select(v => v.Die).ToArray();

            var toAdd = relevantDice.Except(presentedDice);
            var toRemove = presentedDice.Except(relevantDice);
            toAdd.ForEach(Add);
            toRemove.ForEach(Remove);
        }

        private void Add(Die die)
        {
            var view = _container.AddChild<Die2dView>(_prefab.gameObject);
            view.SetDie(die);
            view.gameObject.GetComponentInChildren<DieClickEventRaiser>().SetDie(die);
            
            _views.Add(view);
        }

        private void Remove(Die die)
        {
            var view = _views.FirstOrDefault(v => v.Die == die);
            _views.Remove(view);

            view?.DestroySelf();
        }

        public Die2dView GetView(Guid dieId) => _views.FirstOrDefault(v => v.Die.Id == dieId);
    }
}