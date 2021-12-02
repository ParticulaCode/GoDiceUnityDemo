using FrostLib.Commands.Routined;
using FrostLib.Services;

namespace FrostLib.Commands
{
    public class ScheduleCommand : ICommand
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static Runner Runner => Servicer.Get<Runner>();

        private readonly Routined.ICommand _routinedCmd;
        private readonly ICommand _cmd;

        public ScheduleCommand(Routined.ICommand cmd) => _routinedCmd = cmd;
        
        public ScheduleCommand(ICommand cmd) => _cmd = cmd;

        public void Execute()
        {
            if (_routinedCmd != null)
            {
                Runner.Schedule(_routinedCmd);
                return;
            }

            Runner.Schedule(new ScheduledActionCommand(_cmd.Execute));
        }
    }
}