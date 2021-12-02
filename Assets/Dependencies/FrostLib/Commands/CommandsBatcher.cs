namespace FrostLib.Commands
{
    public class CommandsBatcher
    {
        private readonly ICommand[] _commands;

        public CommandsBatcher(params ICommand[] commands) => _commands = commands;

        public void Execute()
        {
            foreach (var command in _commands)
                command.Execute();
        }
    }
}