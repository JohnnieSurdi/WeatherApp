using Microsoft.AspNetCore.Mvc;
using WeatherApp.Logging;
using WeatherApp.Repositories;
using WeatherApp.Services;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherService _weatherService;
        private readonly IWeatherSearchRepository _weatherSearchRepository;
        private readonly WeatherApp.Logging.ILogger _logger;

        public WeatherController(IWeatherService weatherService, IWeatherSearchRepository weatherSearchRepository, WeatherApp.Logging.ILogger logger)
        {
            _weatherService = weatherService;
            _weatherSearchRepository = weatherSearchRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RecentSearches()
        {
            var allSearches = await _weatherSearchRepository.GetRecentWeatherSearchesAsync();
            return View(allSearches);
        }

        [HttpPost]
        public async Task<IActionResult> SearchByCity(string cityName)
        {
            _logger.Info($"Searching for city: {cityName}");

            var coordinates = await _weatherService.SearchByCityAsync(cityName);

            if (coordinates.lat.HasValue && coordinates.lon.HasValue)
            {
                var weather = await _weatherService.GetWeatherAsync(coordinates.lat.Value, coordinates.lon.Value);
                return View("WeatherView", weather);
            }
            _logger.Error($"Error searching for city {cityName}");
            ViewBag.ErrorMessage = "City not found.";
            return View("Index");
        }
    }
}
