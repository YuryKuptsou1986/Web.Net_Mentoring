using AutoMapper;
using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using ViewModel.Product;

namespace HomeworkWebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ProductControllerApi : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductControllerApi(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a single product by id
        /// </summary>
        /// <response code="200">The product was found</response>
        /// <response code="404">The product was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("{productId:int}", Name = nameof(GetProductById))]
        [ProducesResponseType(typeof(ProductViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductById([FromRoute] int productId)
        {
            var product = await _productService.GetAsync(productId);
            return Ok(product);
        }

        /// <summary>
        /// Get a paginated list of products. The pagination metadata is contained within 'x-pagination' header of response.
        /// </summary>
        /// <response code="200">A paginated list of products</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet(Name = nameof(GetProductList))]
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProductList()
        {
            var products = await _productService.GetAllAsync().ConfigureAwait(false);
            return Ok(products);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <response code="204">The product was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The product was not found for specified product id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        [HttpPut(Name = nameof(UpdateProduct))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateModel product)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!(await _productService.ExistsAsync(product.ProductId))) {
                return NotFound();
            }

            await _productService.UpdateAsync(product);
            return NoContent();
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <response code="201">The product was created successfully/response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPost(Name = nameof(CreateProduct))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateModel product)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var id = await _productService.AddAsync(product);
            return CreatedAtRoute(nameof(GetProductById), new { productId = id }, product);
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <response code="204">The product was deleted successfully.</response>
        /// <response code="404">A product having specified product id was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpDelete("{productId:int}", Name = nameof(DeleteProduct))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
        {
            if (!(await _productService.ExistsAsync(productId))) {
                return NotFound();
            }

            await _productService.DeleteAsync(productId);
            return NoContent();
        }
    }
}
