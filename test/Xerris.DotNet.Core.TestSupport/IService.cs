using System;

namespace Xerris.DotNet.Core.TestSupport;

public interface IService
{
    void DoIt();
}

public class MyService : IService
{
    public void DoIt()
    {
        Console.WriteLine("Hi");
    }
}