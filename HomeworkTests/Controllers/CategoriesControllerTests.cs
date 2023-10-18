using AutoMapper;
using Homework.Entities.ViewModel.Category;
using Homework.Mapper;
using Homework.Services.Interfaces;
using HomeWork_Introduction.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace HomeworkTests.Controllers
{
    [TestFixture]
    public class CategoriesControllerTests
    {
        [Test]
        public async Task Index_ReturnViewResult_Categories()
        {
            // Arrange
            var mapperProfile = new AppMapperConfiguration();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(mapperConfiguration);

            var mockLogger = new Mock<ILogger<CategoriesController>>();
            var mockCategoryService = new Mock<ICategoryService>();
            var mockImageConverterService = new Mock<INorthwindImageConverterService>();
            var categories = CreateCategoriesList();
            mockCategoryService.Setup(x=>x.GetAllAsync()).ReturnsAsync(categories);

            var controller = new CategoriesController(mockLogger.Object, mapper, mockCategoryService.Object, mockImageConverterService.Object);

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
