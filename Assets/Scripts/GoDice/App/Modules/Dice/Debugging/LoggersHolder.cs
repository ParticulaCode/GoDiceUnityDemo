using System;
using System.Collections.Generic;
using System.Linq;
using GoDice.App.Modules.Dice.Core;

namespace GoDice.App.Modules.Dice.Debugging
{
    internal class LoggersHolder : IDisposable, ILoggersHolder
    {
        private readonly List<DieLogger> _loggers = new List<DieLogger>();

        public void Add(Die die) => _loggers.Add(new DieLogger(die));

        public void Remove(Die die)
        {
            var logger = Get(die.Id);
            if (logger == null)
                return;

            _loggers.Remove(logger);
            logger.Dispose();
        }

        public DieLogger Get(Guid dieId) => _loggers.FirstOrDefault(dl => dl.Die.Id == dieId);

        public void Dispose()
        {
            _loggers.ForEach(dl => dl.Dispose());
            _loggers.Clear();
        }
    }
}