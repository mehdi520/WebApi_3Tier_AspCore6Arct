using AlphaApp.ApplicationServices.Contracts;
using AlphaApp.ApplicationServices.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlphaApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IUserService _userService;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService user)
        {
            _logger = logger;
            _userService = user;
        }



        [HttpGet(Name = "GetWeatherForecast")]
        public string Get()
        {
          return _userService.getSomething();
        }
    }
}