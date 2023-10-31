using Homework.Services.Interfaces;
using HomeWork_Introduction.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;
        private readonly INorthwindImageConverterService _imageConverterService;

        public ImageController(ILogger<CategoriesController> logger,ICategoryService categoryService, INorthwindImageConverterService imageConverterService)
        { 
            _logger = logger;
            _categoryService = categoryService;
            _imageConverterService = imageConverterService;
        }

        public async Task<IActionResult> Index(int image_id)
        {
            var category = await _categoryService.GetAsync(image_id);
            var image = _imageConverterService.ConvertToNormalImage(category.Picture);

            return File(image, "image/bmp");
        }
    }
}
