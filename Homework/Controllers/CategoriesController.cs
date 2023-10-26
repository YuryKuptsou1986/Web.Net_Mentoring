using AutoMapper;
using BLL.Services.Interfaces;
using Homework.Entities.ViewModel.Category;
using Homework.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Category;

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

            var categoryPageUpdateModel = _mapper.Map<CategoryPageUpdateModel>(category);

            return View(categoryPageUpdateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId, CategoryName, Description, Picture, FormFile")] CategoryPageUpdateModel categoryPageUpdateModel)
        {
            // ToDo not working
            if (id != categoryPageUpdateModel.CategoryId) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                if (categoryPageUpdateModel.FormFile != null) {
                    categoryPageUpdateModel.Picture = _imageConverterService.ConvertToNorthwindImage(categoryPageUpdateModel.FormFile);
                }

                try {
                    var categoryUpdateModel = _mapper.Map<CategoryUpdateModel>(categoryPageUpdateModel);
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
