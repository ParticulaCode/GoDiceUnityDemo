using FrostLib.Commands;
using FrostLib.Services;
using GoDice.App.Modules.Bluetooth.Devices;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Data;
using GoDice.App.Modules.Dice.Debugging;
using GoDice.App.Modules.Dice.Led;
using GoDice.App.Modules.Dice.Messaging;
using GoDice.App.Modules.Dice.Shells;
using GoDice.Shared.Data;

namespace GoDice.App.Modules.Dice.Commands
{
    internal class CreateDieCommand : ICommand<Die>
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static Holder DiceHolder => Servicer.Get<Holder>();
        private static ILoggersHolder Loggers => Servicer.Get<ILoggersHolder>();
        
        private readonly Data.DieData _data;
        private readonly IDevice _device;

        public CreateDieCommand(Data.DieData data, IDevice device)
        {
            _data = data;
            _device = device;
        }

        public Die Execute()
        {
            var writer = new Writer(_device);
            var die = new Die(_data, _device,
                new LedController(writer),
                new ShellController(),
                new Reader(),
                writer);

            //Add to all dice first, before it may fire OnDieConnected event.
            //Because handlers may ask for all connected dice
            DiceHolder.Add(die);
            Loggers.Add(die);
            
            die.CheckConnection();
            UpdateColorIfNone(die);

            return die;
        }

        private static void UpdateColorIfNone(Die die)
        {
            if (die.Color != ColorType.None)
                return;

            die.RequestColor();
        }
    }
}