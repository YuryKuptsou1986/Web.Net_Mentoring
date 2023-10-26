using AutoMapper;
using BLL.Services.Interfaces;
using Homework.Entities.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using ViewModel.Category;
using ViewModel.Product;
using ViewModel.Supplier;

namespace HomeWork_Introduction.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IOptions<ProductSettings> _productSettings;

        public ProductsController(
            ILogger<ProductsController> logger,
            IMapper mapper,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IProductService productService,
            IOptions<ProductSettings> productSettings)
        {
            _logger = logger;
            _mapper = mapper;
            _supplierService = supplierService;
            _categoryService = categoryService;
            _productService = productService;
            _productSettings = productSettings;
        }

        public async Task<IActionResult> Index()
        {
            var maxRow = _productSettings.Value.MaximumProductRow;

            var products = maxRow > 0
                ? await _productService.GetTopAsync(_productSettings.Value.MaximumProductRow)
                : await _productService.GetAllAsync();

            return View(products);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetAsync(id);
            
            if (product == null) {
                return NotFound();
            }

            var productUpdateModel = _mapper.Map<ProductUpdateModel>(product);

            await AddViewBag(productUpdateModel.CategoryId, productUpdateModel.SupplierId);

            return View(productUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductUpdateModel productUpdateModel)
        {
            if (id != productUpdateModel.ProductId) {
                return NotFound();
            }

            await ValidateProductModel(productUpdateModel);

            if (ModelState.IsValid) {
                try {
                    await _productService.UpdateAsync(productUpdateModel);
                } catch (Exception) {
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await AddViewBag(productUpdateModel.CategoryId, productUpdateModel.SupplierId);

            return View(productUpdateModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            await AddViewBag(null, null);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] ProductCreateModel productCreateModel)
        {
            await ValidateProductModel(productCreateModel);

            if (ModelState.IsValid) {
                await _productService.AddAsync(productCreateModel);
                return RedirectToAction(nameof(Index));
            }
            await AddViewBag(productCreateModel.CategoryId, productCreateModel.SupplierId);

            return View(productCreateModel);
        }

        private async Task AddViewBag(int? categoryId, int? supplierId)
        {
            ViewData["CategoryId"] = new SelectList(await GetCategories(), "CategoryId", "CategoryName", categoryId);
            ViewData["SupplierId"] = new SelectList(await GetSuppliers(), "SupplierId", "CompanyName", supplierId);
        }

        private async Task<IEnumerable<CategoryViewModel>> GetCategories()
        {
            return await _categoryService.GetAllAsync();
        }

        private async Task<IEnumerable<SupplierViewModel>> GetSuppliers()
        {
            return await _supplierService.GetAllAsync();
        }

        private async Task<bool> ValidateProductModel(ProductBaseModel product)
        {
            var supplierValid = await ValidateCategory(product);
            var categoryValid = await ValidateSupplier(product);

            return (supplierValid) && (categoryValid);
        }

        private async Task<bool> ValidateSupplier(ProductBaseModel product)
        {
            if(product == null ||  product.SupplierId == null) {
                return true;
            }

            var exist = await _supplierService.ExistsAsync((int)product.SupplierId);
            if (!exist) {
                ModelState.AddModelError(nameof(product.SupplierId), "Please select correct supplier.");
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateCategory(ProductBaseModel product)
        {
            if (product == null || product.CategoryId == null) {
                return true;
            }

            var exist = await _categoryService.ExistsAsync((int)product.CategoryId);
            if (!exist) {
                ModelState.AddModelError(nameof(product.CategoryId), "Please select correct category.");
                return false;
            }

            return true;
        }
    }
}
