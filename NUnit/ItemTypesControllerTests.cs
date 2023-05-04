using CafeWebApplication;
using CafeWebApplication.Controllers;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NUnit
{
    [TestFixture]
    [Category("MenuTests")]
    public class ItemTypesControllerTests
    {
        private DB_CafeContext _context;
        private ItemTypesController _controller;

        [SetUp]
        public void SetUp()
        {
            SeedDatabase seed = new SeedDatabase();
            seed.SeedDB();

            _context = seed._context;
            _controller = new ItemTypesController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfItemTypes()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WhenIdIsValid()
        {
            // Arrange
            var itemType = await _context.ItemTypes.FirstOrDefaultAsync();
            var expectedViewName = "Index";
            var expectedRouteValues = new { id = itemType.Id, name = itemType.Type };

            // Act
            var result = await _controller.Details(itemType.Id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedViewName, result.ActionName);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsNotFound_WhenItemTypeIsNull()
        {
            // Arrange
            var itemType = new ItemType { Id = 999, Type = "Type999" };

            // Act
            var result = await _controller.Details(itemType.Id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task CreatePOST_WithValidModel_AddsItemTypeAndRedirectsToIndex()
        {
            var itemType = new ItemType { Id = 4, Type = "Type 4" };

            // Act
            var result = await _controller.Create(itemType);

            // Assert
            var itemTypes = _context.ItemTypes.ToList();
            Assert.AreEqual(4, itemTypes.Count);
            Assert.IsTrue(itemTypes.Contains(itemType));
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task CreatePOST_WithInvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var itemType = new ItemType { Id = 4, Type = "" };
            _controller.ModelState.AddModelError("Type", "The Type field is required.");

            // Act
            var result = await _controller.Create(itemType);

            // Assert
            var itemTypes = _context.ItemTypes.ToList();
            Assert.AreEqual(3, itemTypes.Count);
            Assert.IsFalse(itemTypes.Contains(itemType));
            Assert.IsInstanceOf<ViewResult>(result);
            var model = ((ViewResult)result).ViewData.Model;
            Assert.IsInstanceOf<ItemType>(model);
            Assert.AreEqual(itemType, model);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenItemTypeIsNull()
        {
            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsViewResult_WhenIdIsValid()
        {
            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Edit_ReturnsNotFound_WhenIdDoesNotMatch()
        {
            // Arrange
            var itemType = new ItemType { Id = 1, Type = "Updated ItemType" };

            // Act
            var result = await _controller.Edit(4, itemType);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenItemTypeIsNull()
        {
            // Arrange
            int id = 4;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WhenItemTypeIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesItemTypeAndRedirectsToIndex_WhenItemTypeIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
            Assert.IsFalse(_context.ItemTypes.Any(c => c.Id == id));
        }
    }
}
