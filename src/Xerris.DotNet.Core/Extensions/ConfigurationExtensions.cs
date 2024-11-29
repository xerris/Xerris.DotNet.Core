using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Xerris.DotNet.Core.Extensions;

public static class ConfigurationExtensions
{
    public static bool IsNotEmpty(this IConfiguration configuration)
    {
        return configuration.GetChildren().Any();
    }

    public static bool IsEmpty(this IConfiguration configuration)
    {
        return configuration.IsNotEmpty() == false;
    }
}