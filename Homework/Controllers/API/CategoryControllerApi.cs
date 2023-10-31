using AutoMapper;
using BLL.Services.Interfaces;
using Domain.Entities;
using Homework.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ViewModel.Category;

namespace HomeworkWebApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly INorthwindImageConverterService _imageConverterService;

        public CategoryController(ICategoryService categoryService, IMapper mapper, INorthwindImageConverterService imageConverterService)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _imageConverterService = imageConverterService;
        }

        /// <summary>
        /// Get a single category by id
        /// </summary>
        /// <response code="200">The category was found</response>
        /// <response code="404">The category was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("{categoryId:int}", Name = nameof(GetCategoryById))]
        [ProducesResponseType(typeof(CategoryViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryById([FromRoute] int categoryId)
        {
            var category = await _categoryService.GetAsync(categoryId);
            return Ok(category);
        }

        /// <summary>
        /// Get a paginated list of categories. The pagination metadata is contained within 'x-pagination' header of response.
        /// </summary>
        /// <response code="200">A paginated list of categories</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet(Name = nameof(GetCategoryList))]
        [ProducesResponseType(typeof(IEnumerable<CategoryViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoryList()
        {
            var categories = await _categoryService.GetAllAsync().ConfigureAwait(false);

            foreach (var item in categories)
            {
                item.Picture = _imageConverterService.ConvertToNormalImage(item.Picture);
            }
            return Ok(categories);
        }

        /// <summary>
        /// Get an image.
        /// </summary>
        /// <response code="200">An image.</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("image/{imageId:int}", Name = nameof(GetImage))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetImage(int imageId)
        {
            var category = await _categoryService.GetAsync(imageId).ConfigureAwait(false);

            var image = _imageConverterService.ConvertToNormalImage(category.Picture);

            return File(image, "image/bmp");
        }

        /// <summary>
        /// Update an existing image
        /// </summary>
        /// <response code="204">The image was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The image was not found for specified image id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        [HttpPut("image/{imageId:int}", Name = nameof(UpdateImage))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateImage([FromRoute] int imageId, [FromBody] byte[] image)
        {
            image = _imageConverterService.ConvertToNorthwindImage(image);
            await _categoryService.UpdateImage(imageId, image);
            return NoContent();
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        /// <response code="204">The category was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The category was not found for specified category id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        [HttpPut(Name = nameof(UpdateCategory))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryUpdateModel category)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!(await _categoryService.ExistsAsync(category.CategoryId))) {
                return NotFound();
            }

            category.Picture = _imageConverterService.ConvertToNorthwindImage(category.Picture);

            await _categoryService.UpdateAsync(category);
            return NoContent();
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        /// <response code="201">The category was created successfully/response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPost(Name = nameof(CreateCategory))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateModel category)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var id = await _categoryService.AddAsync(category);
            return CreatedAtRoute(nameof(GetCategoryById), new { categoryId = id }, category);
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <response code="204">The category was deleted successfully.</response>
        /// <response code="404">A category having specified category id was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpDelete("{categoryId:int}", Name = nameof(DeleteCategory))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            if (!(await _categoryService.ExistsAsync(categoryId))) {
                return NotFound();
            }

            await _categoryService.DeleteAsync(categoryId);
            return NoContent();
        }
    }
}
