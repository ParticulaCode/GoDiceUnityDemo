using System.Threading.Tasks;

namespace FrostLib.Commands
{
    public interface ICommand
    {
        void Execute();
    }

    public interface ICommand<out T>
    {
        T Execute();
    }
    
    public interface ICommandAsync<T>
    {
        Task<T> Execute();
    }
}