using Microsoft.AspNetCore.Mvc;
using Xerris.Sample.Api.Services;

namespace Xerris.Sample.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherService service;

    public WeatherForecastController(IWeatherService service)
    {
        this.service = service;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return service.Get();
    }
}