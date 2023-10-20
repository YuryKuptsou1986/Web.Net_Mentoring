using AutoMapper;
using Homework.Entities.ViewModel.Category;
using Homework.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork_Introduction.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly INorthwindImageConverterService _imageConverterService;

        public CategoriesController(ILogger<CategoriesController> logger,
            IMapper mapper,
            ICategoryService categoryService,
            INorthwindImageConverterService imageConverterService)
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
            _imageConverterService = imageConverterService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync().ConfigureAwait(false);
            return View(categories);
        }

        public async Task<IActionResult> GetImage(int image_id)
        {
            var category = await _categoryService.GetAsync(image_id).ConfigureAwait(false);

            var image = _imageConverterService.ConvertToNormalImage(category.Picture);

            return File(image, "image/bmp");
        }
        
        public async Task<IActionResult> Images(int image_id)
        {
            return await GetImage(image_id).ConfigureAwait(false);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetAsync(id);

            if (category == null) {
                return NotFound();
            }

            var categoryUpdateModel = _mapper.Map<CategoryUpdateModel>(category);

            return View(categoryUpdateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId, CategoryName, Description, Picture, FormFile")] CategoryUpdateModel categoryUpdateModel)
        {
            // ToDo not working
            if (id != categoryUpdateModel.CategoryId) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                if (categoryUpdateModel.FormFile != null) {
                    categoryUpdateModel.Picture = _imageConverterService.ConvertToNorthwindImage(categoryUpdateModel.FormFile);
                }

                try {
                    await _categoryService.UpdateAsync(categoryUpdateModel);
                } catch (Exception ex) {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
