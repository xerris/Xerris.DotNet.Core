using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Core.Commands
{
    public interface ICommand
    {
        void Run();
    }
    
    public interface IWaitedCommand
    {
        Task RunAsync();
    }

    public interface ICommand<in TInput>
    {
        Task RunAsync(TInput data);
    }
}