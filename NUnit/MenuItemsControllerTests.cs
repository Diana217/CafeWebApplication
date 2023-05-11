using CafeWebApplication.Controllers;
using CafeWebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using CafeWebApplication.Interfaces;

namespace NUnit
{
    [TestFixture]
    [Category("MenuTests")]
    public class MenuItemsControllerTests
    {
        private DB_CafeContext _context;
        private Mock<IDBContextFactory> _contextFactoryMock;
        private MenuItemsController _controller;

        [SetUp]
        public void SetUp()
        {
            SeedDatabase seed = new SeedDatabase();
            seed.SeedDB();

            _context = seed._context;
            _contextFactoryMock = seed._contextFactoryMock;

            _controller = new MenuItemsController(_contextFactoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        [TestCase(1, "Dessert", true, TestName = "Test1")]
        [TestCase(2, "Drink", true, TestName = "Test2")]
        [TestCase(1, "", false, TestName = "Test3")]
        [TestCase(0, "Dessert", false, TestName = "Test4")]
        public async Task Index_ReturnsAViewResult_WithAListOfMenuItems(int menuId, string searchTerm, bool expectedResult)
        {
            // Act
            var result = await _controller.Index(menuId, searchTerm);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Details_ReturnsNotFoundResult_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsNotFoundResult_WhenMenuItemIsNotFound()
        {
            // Act
            var result = await _controller.Details(4);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithMenuItem()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Create_Post_ValidModelState_ReturnsRedirectToActionResult()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 4, CafeId = 3, ItemTypeId = 1, Name = "Dessert 5", Price = 75, Status = false };

            // Act
            var result = await _controller.Create(1, menuItem) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var menuItem = new MenuItem { Id = 4, CafeId = 3, ItemTypeId = 1, Name = "", Price = 75, Status = false };

            _controller.ModelState.AddModelError("Name", "Name is required.");

            // Act
            var result = await _controller.Create(1, menuItem) as ViewResult;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Edit_Post_ValidModelState_ReturnsRedirectToActionResult()
        {
            // Arrange
            var menuItem = await _context.MenuItems.FirstOrDefaultAsync();

            menuItem.Name = "Dessert 101";

            // Act
            var result = await _controller.Edit(menuItem.Id, menuItem) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenMenuItemIsNull()
        {
            // Arrange
            int id = 4;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WhenMenuItemIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesMenuItemAndRedirectsToIndex_WhenMenuItemIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
            Assert.IsFalse(_context.MenuItems.Any(c => c.Id == id));
        }
    }
}
