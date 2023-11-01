using AutoMapper;
using BLL.Services.Interfaces;
using HomeworkWebApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Category;

namespace HomeworkWebApiTests.Controllers
{
    [TestFixture]
    internal class CategoryControllerApiTests
    {
        private Mock<ILogger<ProductsApiController>> _loggerMock;
        private IMapper _mapper;
        private Mock<ICategoryService> _categoryServiceMock;
        private IEnumerable<CategoryViewModel> _categories;

        private const int CreatedId = 100;
        private const string Category_1_Name = "Category 1";
        private const string Category_2_Name = "Category 2";

        private const int Category_1_ID = 1;
        private const int Category_2_ID = 2;

        private const string UpdatedCategoryName = "Updated category name";
        private const string NewCategoryName = "New categry name";

        private const string Category_1_Description = "Category description 1";
        private const string Category_2_Description = "Category description 2";

        [SetUp]
        public void SetUp()
        {
            _categories = CreateCategoriesList();
            _loggerMock = new Mock<ILogger<ProductsApiController>>();

            var BllMapperProfile = new BLL.Mappings.AppMappingProfile();
            var MvcMapperProfile = new Homework.Mapper.AppMappingProfile();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(BllMapperProfile); cfg.AddProfile(MvcMapperProfile); });
            _mapper = new Mapper(mapperConfiguration);

            _categoryServiceMock = new Mock<ICategoryService>();
            _categoryServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_categories);
            _categoryServiceMock.Setup(x => x.AddAsync(It.IsAny<CategoryCreateModel>())).ReturnsAsync(CreatedId);
            _categoryServiceMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync((int id) => _categories.FirstOrDefault(x => x.CategoryId == id));
        }

        [Test]
        public async Task GetCategoryById_ReturnCategory()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);

            // Act
            var result = await categoriesControllerApi.GetCategoryById(2);

            // Assert
            var contentResult = result as OkObjectResult;
            Assert.IsNotNull(contentResult);
            var model = contentResult.Value as CategoryViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(Category_2_ID, model.CategoryId);
            Assert.AreEqual(Category_2_Name, model.CategoryName);
        }

        [Test]
        public async Task GetAllCategories_ReturnCategories()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);

            // Act
            var result = await categoriesControllerApi.GetCategoryList();

            // Assert
            var contentResult = result as OkObjectResult;
            Assert.IsNotNull(contentResult);
            var models = contentResult.Value as IEnumerable<CategoryViewModel>;
            Assert.IsNotNull(models);
            Assert.AreEqual(_categories.Count(), models.Count());
        }

        [Test]
        public async Task UpdateCategory_ServiceUpdateLogicCalled()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);
            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var category = _categories.First();
            var updatedCategory = _mapper.Map<CategoryUpdateModel>(category);
            updatedCategory.CategoryName = UpdatedCategoryName;

            var result = await categoriesControllerApi.UpdateCategory(updatedCategory);

            // Assert
            Assert.IsTrue(result is NoContentResult);
            _categoryServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _categoryServiceMock.Verify(x => x.UpdateAsync(It.Is<CategoryUpdateModel>(x => x.CategoryId == updatedCategory.CategoryId)), Times.Once());
        }

        [Test]
        public async Task UpdateCategory_CategoryNotFound_NotFoundResultReturned()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);
            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var category = _categories.First();
            var updatedCategory = _mapper.Map<CategoryUpdateModel>(category);

            var result = await categoriesControllerApi.UpdateCategory(updatedCategory);

            // Assert
            Assert.IsTrue(result is NotFoundResult);
            _categoryServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _categoryServiceMock.Verify(x => x.UpdateAsync(It.Is<CategoryUpdateModel>(x => x.CategoryId == updatedCategory.CategoryId)), Times.Never());
        }

        [Test]
        public async Task CreateCategory_ServiceCreateLogicCalled_CreatedAtRouteResultReturned()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);

            // Act
            var catetory = _categories.First();
            var newCategory = _mapper.Map<CategoryCreateModel>(catetory);
            newCategory.CategoryName = NewCategoryName;

            var result = await categoriesControllerApi.CreateCategory(newCategory);

            // Assert
            var createdAtRouteResult = result as CreatedAtRouteResult;
            Assert.NotNull(createdAtRouteResult);
            Assert.IsTrue(createdAtRouteResult.RouteValues.Values.Any(x => (int)x == CreatedId));
            Assert.AreEqual(nameof(categoriesControllerApi.GetCategoryById), (result as CreatedAtRouteResult).RouteName);
            _categoryServiceMock.Verify(x => x.AddAsync(It.Is<CategoryCreateModel>(x => x.CategoryName == newCategory.CategoryName)), Times.Once());
        }

        [Test]
        public async Task DeleteCategory_DeleteLogicCalled()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);
            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await categoriesControllerApi.DeleteCategory(Category_2_ID);

            // Assert
            Assert.IsTrue(result is NoContentResult);
            _categoryServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _categoryServiceMock.Verify(x => x.DeleteAsync(It.Is<int>(x => x == Category_2_ID)), Times.Once());
        }

        [Test]
        public async Task DeleteProduct_ProductNotFound_NotFoundResultReturned()
        {
            // Arrange
            var categoriesControllerApi = new CategoriesApiController(_categoryServiceMock.Object, _mapper);
            _categoryServiceMock.Setup(x => x.ExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act
            var result = await categoriesControllerApi.DeleteCategory(Category_2_ID);

            // Assert
            Assert.IsTrue(result is NotFoundResult);
            _categoryServiceMock.Verify(x => x.ExistsAsync(It.IsAny<int>()), Times.Once());
            _categoryServiceMock.Verify(x => x.DeleteAsync(It.Is<int>(x => x == Category_2_ID)), Times.Never());
        }

        private IEnumerable<CategoryViewModel> CreateCategoriesList()
        {
            var categories = new List<CategoryViewModel>() {
                new CategoryViewModel {
                    CategoryId = Category_1_ID,
                    CategoryName = Category_1_Name,
                    Description = Category_1_Description,
                    Picture = null,
                    Products = null
                },
                new CategoryViewModel {
                    CategoryId = Category_2_ID,
                    CategoryName = Category_2_Name,
                    Description = Category_2_Description,
                    Picture = null,
                    Products = null
                }
            };
            return categories;
        }
    }
}
