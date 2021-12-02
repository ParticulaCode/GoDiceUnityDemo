using System;
using System.Collections.Generic;
using System.Linq;
using GoDice.Shared.Data;

namespace GoDice.App.Modules.Dice.Core
{
    internal class Holder : IDiceHolder
    {
        private readonly List<Die> _dice = new List<Die>();

        public void Add(Die die) => _dice.Add(die);

        public IEnumerable<Die> GetDice() => _dice;

        IEnumerable<IDie> IDiceHolder.GetConnectedDice() => _dice.Where(d => d.IsConnected);

        public void RemoveDie(Die die)
        {
            _dice.Remove(die);
            die.Dispose();
        }

        IDie IDiceHolder.GetDie(Guid id) => GetDie(id);

        public Die GetDie(Guid id) => _dice.FirstOrDefault(d => d.Id == id);

        public Die GetDie(ColorType color) => _dice.FirstOrDefault(d => d.Color == color);

        public Die GetDie(string address) => _dice.FirstOrDefault(d => d.Address == address);
    }
}