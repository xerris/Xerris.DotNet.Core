using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;

namespace Xerris.Sample.Api.Services
{
    public interface IWeatherService
    {
        IEnumerable<WeatherForecast> Get();
    }

    public class WeatherService : IWeatherService
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        public IEnumerable<WeatherForecast> Get()
        {
            Log.Information("Getting today's weather");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }
    }
}