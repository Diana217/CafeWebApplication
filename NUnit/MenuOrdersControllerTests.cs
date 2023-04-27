using CafeWebApplication.Controllers;
using CafeWebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace NUnit
{
    [TestFixture]
    public class MenuOrdersControllerTests
    {
        private DB_CafeContext _context;
        private MenuOrdersController _controller;

        [SetUp]
        public void SetUp()
        {
            SeedDatabase seed = new SeedDatabase();
            seed.SeedDB();

            _context = seed._context;
            _controller = new MenuOrdersController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfMenuOrders()
        {
            // Act
            var result = await _controller.Index(1);

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
        public async Task Details_ReturnsNotFoundResult_WhenMenuOrderIsNotFound()
        {
            // Act
            var result = await _controller.Details(4);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithMenuOrder()
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
            var menuOrder = new MenuOrder { Id = 4, MenuItemId = 1, OrderId = 1, Amount = 2 };

            // Act
            var result = await _controller.Create(1, menuOrder) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var menuOrder = new MenuOrder { Id = 4, MenuItemId = 1, OrderId = 1 };

            _controller.ModelState.AddModelError("Amount", "Amount is required.");

            // Act
            var result = await _controller.Create(1, menuOrder) as ViewResult;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task Edit_Post_ValidModelState_ReturnsRedirectToActionResult()
        {
            // Arrange
            var menuOrder = await _context.MenuOrders.FirstOrDefaultAsync();

            menuOrder.Amount = 5;

            // Act
            var result = await _controller.Edit(menuOrder.Id, menuOrder) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenMenuOrderIsNull()
        {
            // Arrange
            int id = 4;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WhenMenuOrderIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesMenuOrderAndRedirectsToIndex_WhenMenuOrderIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
            Assert.IsFalse(_context.MenuOrders.Any(c => c.Id == id));
        }
    }
}
