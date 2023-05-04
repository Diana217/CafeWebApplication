using CafeWebApplication.Controllers;
using CafeWebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace NUnit
{
    [TestFixture]
    [Category("CafeTests")]
    public class TablesControllerTests
    {
        private DB_CafeContext _context;
        private TablesController _controller;

        [SetUp]
        public void SetUp()
        {
            SeedDatabase seed = new SeedDatabase();
            seed.SeedDB();

            _context = seed._context;
            _controller = new TablesController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfTables()
        {
            // Act
            var result = await _controller.Index();

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
        public async Task Details_ReturnsNotFoundResult_WhenTableIsNotFound()
        {
            // Act
            var result = await _controller.Details(4);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithTable()
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
            var table = new Table { Id = 4, CafeId = 1, Status = true, Number = 5 };

            // Act
            var result = await _controller.Create(table) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var table = new Table { Id = 4, CafeId = 1, Status = true };

            _controller.ModelState.AddModelError("Number", "Number is required.");

            // Act
            var result = await _controller.Create(table) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(table, result.Model);
        }

        [Test]
        public async Task Edit_Post_ValidModelState_ReturnsRedirectToActionResult()
        {
            // Arrange
            var table = await _context.Tables.FirstOrDefaultAsync();

            table.Number = 7;

            // Act
            var result = await _controller.Edit(table.Id, table) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenTablesIsNull()
        {
            // Arrange
            int id = 4;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WhenTablesIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesTablesAndRedirectsToIndex_WhenTablesIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
            Assert.IsFalse(_context.Tables.Any(c => c.Id == id));
        }
    }
}
