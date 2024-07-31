using FrostLib.Commands.Routined;
using FrostLib.Services;

namespace FrostLib.Commands
{
    public class ScheduleCommand : ICommand
    {
        private static ServiceLocator Servicer => ServiceLocator.Instance;
        private static Runner Runner => Servicer.Get<Runner>();

        private readonly IRoutinedCommand _routinedCmd;
        private readonly ICommand _cmd;

        public ScheduleCommand(IRoutinedCommand cmd) => _routinedCmd = cmd;

        public ScheduleCommand(ICommand cmd) => _cmd = cmd;

        public void Execute()
        {
            if (_routinedCmd != null)
            {
                Runner.Schedule(_routinedCmd);
                return;
            }

            Runner.Schedule(new ScheduledActionRoutinedCommand(_cmd.Execute));
        }
    }
}