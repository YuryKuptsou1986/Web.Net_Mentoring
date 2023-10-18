using AutoMapper;
using Homework.Entities.Configuration;
using Homework.Entities.ViewModel.Category;
using Homework.Entities.ViewModel.Product;
using Homework.Entities.ViewModel.Supplier;
using Homework.Mapper;
using Homework.Services.Interfaces;
using HomeWork_Introduction.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace HomeworkTests.Controllers
{
    [TestFixture]
    public class ProductsControllerTests
    {
        private Mock<ILogger<ProductsController>> _loggerMock;
        private IMapper _mapper;
        private Mock<IProductService> _productServiceMock;
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<ISupplierService> _supplierServiceMock;
        private Mock<IOptions<ProductSettings>> _productSettingsMock;

        private IEnumerable<SupplierViewModel> _suppliers;
        private IEnumerable<CategoryViewModel> _categories;
        private IEnumerable<ProductViewModel> _products;

        private const int MaximumProductRow = 1;

        [SetUp]
        public void SetUp()
        {
            SetupEntities();
            _loggerMock = new Mock<ILogger<ProductsController>>();

            var mapperProfile = new AppMapperConfiguration();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            _mapper = new Mapper(mapperConfiguration);

            _productServiceMock = new Mock<IProductService>();
            _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_products);
            _productServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _products.FirstOrDefault(x => x.ProductId == id));
            _productServiceMock.Setup(x => x.GetTopAsync(MaximumProductRow)).ReturnsAsync(_products.Take(MaximumProductRow));
            _productServiceMock.Setup(x => x.UpdateAsync(It.IsAny<ProductUpdateModel>()));
            _productServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>()));
            _productServiceMock.Setup(x => x.AddAsync(It.IsAny<ProductCreateModel>()));

            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_categories);
            
            _supplierServiceMock = new Mock<ISupplierService>();
            _supplierServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_suppliers);

            _productSettingsMock = new Mock<IOptions<ProductSettings>>();
        }

        [TestCase(1,1)]
        [TestCase(0, 4)]
        public async Task Index_ReturnViewResult_Products(int maximumRowSettings, int productCountFromService)
        {
            // Arrange
            _productSettingsMock.Setup(x => x.Value).Returns(new ProductSettings { MaximumProductRow = maximumRowSettings });

            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOf<IEnumerable<ProductViewModel>>(viewResult.ViewData.Model);
            var model = viewResult.ViewData.Model as IEnumerable<ProductViewModel>;

            Assert.AreEqual(productCountFromService, model.Count());
            Assert.True(model.All(x => x.Category != null));
            Assert.True(model.All(x => x.Supplier != null));
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Edit_ReturnUpdateResult_Products(int productId)
        {
            // Arrange
            
            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            // Act
            var result = await controller.Edit(productId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOf<ProductUpdateModel>(viewResult.ViewData.Model);
            var model = viewResult.ViewData.Model as ProductUpdateModel;

            Assert.AreEqual(productId, model.ProductId);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task EditPost_ReturnRedirectResult_WhenModelStateValid(int productId)
        {
            // Arrange

            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync((int categoryId) => _categories.Any(x => x.CategoryId == categoryId));
            _supplierServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync((int supplierId) => _suppliers.Any(x => x.SupplierId == supplierId));

            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            var productUpdate = _mapper.Map<ProductUpdateModel>(_products.FirstOrDefault(x => x.ProductId == productId));

            // Act
            var result = await controller.Edit(productId, productUpdate);

            // Assert
            _productServiceMock.Verify(x => x.UpdateAsync(It.IsAny<ProductUpdateModel>()), Times.Once);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;

            Assert.Null(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task EditPost_ReturnUpdateResult_WhenModelStateInValid(int productId)
        {
            // Arrange
            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            var productUpdate = _mapper.Map<ProductUpdateModel>(_products.FirstOrDefault(x => x.ProductId == productId));

            // Act
            var result = await controller.Edit(productId, productUpdate);

            // Assert
            _productServiceMock.Verify(x => x.UpdateAsync(It.IsAny<ProductUpdateModel>()), Times.Never);

            Assert.False(controller.ModelState.IsValid);
            Assert.True(controller.ModelState.Any(x => x.Value.Errors.Any(m=>m.ErrorMessage.Equals("Please select correct supplier."))));
            Assert.True(controller.ModelState.Any(x => x.Value.Errors.Any(m => m.ErrorMessage.Equals("Please select correct category."))));

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOf<ProductUpdateModel>(viewResult.ViewData.Model);
            var model = viewResult.ViewData.Model as ProductUpdateModel;

            Assert.AreEqual(productId, model.ProductId);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Delete_ReturnRedirectResult(int productId)
        {
            // Arrange
            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            // Act
            var result = await controller.Delete(productId);

            // Assert
            _productServiceMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;

            Assert.Null(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Create_ReturnEmptyViewResult()
        {
            // Arrange
            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            // Act
            var result = await controller.Create();

            // Assert

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.Null(viewResult.ViewData.Model);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task CreatePost_ReturnRedirectResult_WhenModelStateValid(int productId)
        {
            // Arrange
            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            // Act
            var createProduct = _mapper.Map<ProductCreateModel>(_products.FirstOrDefault(x=>x.ProductId == productId));

            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync((int categoryId) => _categories.Any(x => x.CategoryId == createProduct.CategoryId));
            _supplierServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync((int supplierId) => _suppliers.Any(x => x.SupplierId == createProduct.SupplierId));

            var result = await controller.Create(createProduct);

            // Assert
            _productServiceMock.Verify(x => x.AddAsync(It.Is<ProductCreateModel>(x=>
                x.SupplierId == createProduct.SupplierId && x.ProductName == createProduct.ProductName)), Times.Once);

            Assert.True(controller.ModelState.IsValid);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;

            Assert.Null(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task CreatePost_ReturnCreateResult_WhenModelStateInValid(int productId)
        {
            // Arrange
            var controller = new ProductsController(
                _loggerMock.Object,
                _mapper,
                _categoryServiceMock.Object,
                _supplierServiceMock.Object,
                _productServiceMock.Object,
                _productSettingsMock.Object
                );

            // Act
            var createProduct = _mapper.Map<ProductCreateModel>(_products.FirstOrDefault(x => x.ProductId == productId));

            controller.ModelState.AddModelError("ProductName", "Required");
            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync((int categoryId) => _categories.Any(x => x.CategoryId == createProduct.CategoryId));
            _supplierServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync((int supplierId) => _suppliers.Any(x => x.SupplierId == createProduct.SupplierId));

            var result = await controller.Create(createProduct);

            // Assert
            _productServiceMock.Verify(x => x.AddAsync(It.Is<ProductCreateModel>(x =>
                x.SupplierId == createProduct.SupplierId && x.ProductName == createProduct.ProductName)), Times.Never);

            Assert.False(controller.ModelState.IsValid);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOf<ProductCreateModel>(viewResult.ViewData.Model);
            var model = viewResult.ViewData.Model as ProductCreateModel;

            Assert.AreEqual(createProduct.ProductName, model.ProductName);
        }

        private IEnumerable<CategoryViewModel> SetupCategories(int count)
        {
            var list = new List<CategoryViewModel>();

            for(int i = 0; i < count; i++) {
                list.Add(CreateCategory(i + 1));
            }

            return list;
        }

        private IEnumerable<SupplierViewModel> SetupSuppliers(int count)
        {
            var list = new List<SupplierViewModel>();

            for (int i = 0; i < count; i++) {
                list.Add(CreateSupplier(i + 1));
            }

            return list;
        }

        private void SetupEntities()
        {
            int countCategories = 2;
            int countSuppliers = 2;

            var categories = SetupCategories(countCategories).ToList();
            var suppliers = SetupSuppliers(countSuppliers).ToList();

            var products = new List<ProductViewModel>();

            int productId = 1;
            for (int cat = 0; cat< countCategories; cat++) {
                for (int sup = 0; sup < countSuppliers; sup++) {
                    products.Add(CreateProduct(productId, categories[cat], suppliers[sup]));
                    productId++;
                }
            }

            _categories = categories;
            _suppliers = suppliers;
            _products = products;
        }

        private ProductViewModel CreateProduct(int productId, CategoryViewModel category, SupplierViewModel supplier)
        {
            return new ProductViewModel {
                Category = category,
                ProductName = "Product" + productId,
                CategoryId = category.CategoryId,
                ProductId = productId,
                SupplierId = supplier.SupplierId,
                Supplier = supplier
            };
        }

        private SupplierViewModel CreateSupplier(int supplierId)
        {
            return new SupplierViewModel {
                Address = "Address" + supplierId,
                CompanyName = "CompanyName" + supplierId,
                SupplierId = supplierId
            };
        }

        private CategoryViewModel CreateCategory(int categoryId)
        {
            return new CategoryViewModel {
                CategoryId = categoryId,
                CategoryName = "Category1",
                Description = "Category1 description",
                Picture = null,
                
            };
        }
    }
}
