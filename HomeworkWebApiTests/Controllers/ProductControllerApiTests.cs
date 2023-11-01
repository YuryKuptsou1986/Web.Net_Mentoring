using AutoMapper;
using BLL.Services.Interfaces;
using Homework.Entities.Configuration;
using HomeworkWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ViewModel.Category;
using ViewModel.Product;
using ViewModel.Supplier;

namespace HomeworkWebApiTests.Controllers
{
    [TestFixture]
    internal class ProductControllerApiTests
    {
        private Mock<ILogger<ProductsApiController>> _loggerMock;
        private IMapper _mapper;
        private Mock<IProductService> _productServiceMock;
        private Mock<ICategoryService> _categoryServiceMock;
        private Mock<ISupplierService> _supplierServiceMock;
        private Mock<IOptions<ProductSettings>> _productSettingsMock;

        private IEnumerable<SupplierViewModel> _suppliers;
        private IEnumerable<CategoryViewModel> _categories;
        private IEnumerable<ProductViewModel> _products;

        private const int CreatedId = 100;
        private const int DeletedId = 4;
        private const int LastProductId = 4;

        private const string UpdatedProductName = "Updated product name";
        private const string NewProductName = "New product name";

        [SetUp]
        public void SetUp()
        {
            SetupEntities();
            _loggerMock = new Mock<ILogger<ProductsApiController>>();

            var BllMapperProfile = new BLL.Mappings.AppMappingProfile();
            var MvcMapperProfile = new Homework.Mapper.AppMappingProfile();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(BllMapperProfile); cfg.AddProfile(MvcMapperProfile); });
            _mapper = new Mapper(mapperConfiguration);

            _productServiceMock = new Mock<IProductService>();
            _productServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_products);
            _productServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _products.FirstOrDefault(x => x.ProductId == id));
            _productServiceMock.Setup(x => x.UpdateAsync(It.IsAny<ProductUpdateModel>()));
            _productServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>()));
            _productServiceMock.Setup(x => x.AddAsync(It.IsAny<ProductCreateModel>())).ReturnsAsync(CreatedId);

            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_categories);

            _supplierServiceMock = new Mock<ISupplierService>();
            _supplierServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_suppliers);

            _productSettingsMock = new Mock<IOptions<ProductSettings>>();
        }

        [Test]
        public async Task GetProductById_ReturnProduct()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);

            // Act
            var result = await productControllerApi.GetProductById(LastProductId);

            // Assert
            var contentResult = result as OkObjectResult;
            Assert.IsNotNull(contentResult);
            var model = contentResult.Value as ProductViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(LastProductId, model.ProductId);
            Assert.NotNull(model.Category);
            Assert.NotNull(model.Supplier);
        }

        [Test]
        public async Task GetAllProducts_ReturnProducts()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);

            // Act
            var result = await productControllerApi.GetProductList();

            // Assert
            var contentResult = result as OkObjectResult;
            Assert.IsNotNull(contentResult);
            var model = contentResult.Value as IEnumerable<ProductViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(_products.Count(), model.Count());
            Assert.True(model.All(x => x.Category != null));
            Assert.True(model.All(x => x.Supplier != null));
        }

        [Test]
        public async Task UpdateProduct_ServiceUpdateLogicCalled()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);
            _productServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var product = _products.First();
            var updatedProduct = _mapper.Map<ProductUpdateModel>(product);
            updatedProduct.ProductName = UpdatedProductName;

            var result = await productControllerApi.UpdateProduct(updatedProduct);

            // Assert
            Assert.IsTrue(result is NoContentResult);
            _productServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _productServiceMock.Verify(x=>x.UpdateAsync(It.Is<ProductUpdateModel>(x=>x.ProductId == updatedProduct.ProductId)), Times.Once());
        }

        [Test]
        public async Task UpdateProduct_ProductNotFound_NotFoundResultReturned()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);
            _productServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var product = _products.First();
            var updatedProduct = _mapper.Map<ProductUpdateModel>(product);

            var result = await productControllerApi.UpdateProduct(updatedProduct);

            // Assert
            Assert.IsTrue(result is NotFoundResult);
            _productServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _productServiceMock.Verify(x => x.UpdateAsync(It.Is<ProductUpdateModel>(x => x.ProductId == updatedProduct.ProductId)), Times.Never());
        }

        [Test]
        public async Task CreateProduct_ServiceCreateLogicCalled_CreatedAtRouteResultReturned()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);

            // Act
            var product = _products.First();
            var newProduct = _mapper.Map<ProductCreateModel>(product);
            newProduct.ProductName = NewProductName;

            var result = await productControllerApi.CreateProduct(newProduct);

            // Assert
            var createdAtRouteResult = result as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);
            Assert.IsTrue(createdAtRouteResult.RouteValues.Values.Any(x => (int)x == CreatedId));
            Assert.AreEqual(nameof(productControllerApi.GetProductById), (result as CreatedAtRouteResult).RouteName);
            _productServiceMock.Verify(x => x.AddAsync(It.Is<ProductCreateModel>(x => x.ProductName == newProduct.ProductName)), Times.Once());
        }

        [Test]
        public async Task DeleteProduct_DeleteLogicCalled()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);
            _productServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await productControllerApi.DeleteProduct(DeletedId);

            // Assert
            Assert.IsTrue(result is NoContentResult);
            _productServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _productServiceMock.Verify(x => x.DeleteAsync(It.Is<int>(x => x == DeletedId)), Times.Once());
        }

        [Test]
        public async Task DeleteProduct_ProductNotFound_NotFoundResultReturned()
        {
            // Arrange
            var productControllerApi = new ProductsApiController(_productServiceMock.Object, _mapper);
            _productServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await productControllerApi.DeleteProduct(DeletedId);

            // Assert
            Assert.IsTrue(result is NotFoundResult);
            _productServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _productServiceMock.Verify(x => x.DeleteAsync(It.Is<int>(x => x == DeletedId)), Times.Never());
        }

        private IEnumerable<CategoryViewModel> SetupCategories(int count)
        {
            var list = new List<CategoryViewModel>();

            for (int i = 0; i < count; i++) {
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
            for (int cat = 0; cat < countCategories; cat++) {
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
