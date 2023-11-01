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
        private readonly IFormFileToStreamConverter _formFileToStreamConverter;

        public CategoriesController(ILogger<CategoriesController> logger,
            IMapper mapper,
            ICategoryService categoryService,
            IFormFileToStreamConverter formFileToStreamConverter)
        {
            _logger = logger;
            _mapper = mapper;
            _categoryService = categoryService;
            _formFileToStreamConverter = formFileToStreamConverter;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        public async Task<IActionResult> GetImage(int image_id)
        {
            var category = await _categoryService.GetAsync(image_id);

            return File(category.Picture, "image/bmp");
        }
        
        public async Task<IActionResult> Images(int image_id)
        {
            return await GetImage(image_id);
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
            if (id != categoryPageUpdateModel.CategoryId) {
                return NotFound();
            }
            if (ModelState.IsValid) {
                if (categoryPageUpdateModel.FormFile != null) {
                    categoryPageUpdateModel.Picture = _formFileToStreamConverter.ConvertToStream(categoryPageUpdateModel.FormFile);
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
