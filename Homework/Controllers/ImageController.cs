using BLL.Services.Interfaces;
using HomeWork_Introduction.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Homework.Controllers
{
    public class ImageController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly ICategoryService _categoryService;

        public ImageController(ILogger<CategoriesController> logger,ICategoryService categoryService)
        { 
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int image_id)
        {
            var category = await _categoryService.GetAsync(image_id);

            return File(category.Picture, "image/bmp");
        }
    }
}
