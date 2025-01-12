namespace Xerris.DotNet.Core.Test.DI
{
    public interface IService { }

    public class ServiceImplementation : IService { }
    
    public class DefaultServiceImplementation : IService { }

    public class OriginalServiceImplementation : IService { }

    public class NewServiceImplementation : IService { }
}