using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.WebAPI.Controllers
{/// <summary>
/// Hava durumu iþlemleri ile ilgili API metodlarýný çalýþtýran controller sýnýfý
/// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">DI Injection parametresi</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Tüm hava durumlarýný getiren api metodu
        /// </summary>
        /// <returns>Mevcut tahmini hava durumunu döndürür</returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
