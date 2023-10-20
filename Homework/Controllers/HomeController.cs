using Homework.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Homework.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult ErrorMiddleware(string messageJson)
        {
            var errorMessage = JsonConvert.DeserializeObject<ErrorViewModel>(messageJson);

            return View("Error", errorMessage);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exc = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var errorMessage = new ErrorViewModel {
                Path = exc.Path,
                ErrorMessages = new List<string> { exc.Error.Message }
            };

            return View(errorMessage);
        }
    }
}