namespace Xerris.DotNet.Core.Core.Commands
{
    public interface ICommand
    {
        void Run();
    }

    public interface ICommand<in TInput>
    {
        void Run(TInput data);
    }
}