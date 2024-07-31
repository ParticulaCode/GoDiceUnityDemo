using System;
using GoDice.App.Modules.Bluetooth.Events;
using GoDice.App.Modules.Dice.Commands;
using GoDice.App.Modules.Dice.Core;
using GoDice.App.Modules.Dice.Data;
using GoDice.Shared.EventDispatching.Injections;
using JetBrains.Annotations;

namespace GoDice.App.Modules.Dice
{
    [UsedImplicitly]
    internal class CreateDieForNewDeviceHandler : Shared.EventDispatching.Handlers.EventHandler
    {
        [Inject] private Holder DiceHolder { get; set; }

        public CreateDieForNewDeviceHandler(NewDeviceConnectedEvent e) : base(e)
        {
        }

        public override void Handle()
        {
            var device = EventAs<NewDeviceConnectedEvent>().Device;
            var die = DiceHolder.GetDie(device.Address);
            if (die != null)
                return;

            var data = new DieData
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Name = device.Name
            };

            new CreateDieCommand(data, device).Execute();
        }
    }
}