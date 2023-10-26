using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HomeworkWebApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class ProductControllerApi : Controller
    {
        private readonly IProductService _productService;

        public ProductControllerApi(IProductService productService)
        {
            _productService = productService;
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
        [HttpGet(Name = nameof(GetProductList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
    }
}
