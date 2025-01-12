using Microsoft.Extensions.DependencyInjection;

namespace Xerris.DotNet.Core.DI;

public interface IModule
{
    int Priority { get; }
    void RegisterServices(IServiceCollection services);
}