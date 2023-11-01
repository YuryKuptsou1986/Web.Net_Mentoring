using AutoMapper;
using BLL.Mappings;
using BLL.Services.Interfaces;
using Homework.Services.Interfaces;
using HomeWork_Introduction.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ViewModel.Category;

namespace HomeworkTests.Controllers
{
    [TestFixture]
    public class CategoriesControllerTests
    {
        [Test]
        public async Task Index_ReturnViewResult_Categories()
        {
            // Arrange
            var mapperProfile = new AppMappingProfile();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(mapperConfiguration);

            var mockLogger = new Mock<ILogger<CategoriesController>>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockFormFileConverterService = new Mock<IFormFileToStreamConverter>();
            var categories = CreateCategoriesList();
            mockCategoryService.Setup(x=>x.GetAllAsync()).ReturnsAsync(categories);

            var controller = new CategoriesController(mockLogger.Object, mapper, mockCategoryService.Object, mockFormFileConverterService.Object);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;

            Assert.IsInstanceOf<IEnumerable<CategoryViewModel>>(viewResult.ViewData.Model);

            var model = viewResult.ViewData.Model as IEnumerable<CategoryViewModel>;

            Assert.AreEqual(2, model.Count());
        }

        private IEnumerable<CategoryViewModel> CreateCategoriesList()
        {
            var categories = new List<CategoryViewModel>() {
                new CategoryViewModel {
                    CategoryId = 1,
                    CategoryName = "Category1",
                    Description = "Category1 description",
                    Picture = null,
                    Products = null
                },
                new CategoryViewModel {
                    CategoryId = 2,
                    CategoryName = "Category2",
                    Description = "Category2 description",
                    Picture = null,
                    Products = null
                }
            };
            return categories;
        }
    }
}
