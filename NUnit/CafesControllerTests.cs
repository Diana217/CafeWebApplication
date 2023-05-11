using CafeWebApplication.Controllers;
using CafeWebApplication;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CafeWebApplication.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NuGet.Protocol.Plugins;

namespace NUnit
{
    [TestFixture]
    [Category("ParallelTests")]
    [Parallelizable]
    public class CafesControllerTests
    {
        private DB_CafeContext _context;
        private Mock<IDBContextFactory> _contextFactoryMock;
        private CafesController _controller;

        [SetUp]
        public void SetUp()
        {
            SeedDatabase seed = new SeedDatabase();
            seed.SeedDB();

            _context = seed._context;
            _contextFactoryMock = seed._contextFactoryMock;

            _contextFactoryMock.CallBase = false;
            _controller = new CafesController(_contextFactoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfCafes()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            CollectionAssert.AllItemsAreNotNull(_context.Cafes);
        }
        
        [Test]
        public async Task Details_ArgumentNullException_WhenIdIsNull()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _controller.Details(null));
        }

        [Test]
        public async Task Details_ReturnsNotFoundResult_WhenCafeIsNotFound()
        {
            // Act
            var result = await _controller.Details(4);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithCafe()
        {
            // Act
            var result = await _controller.Details(1);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
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
        public async Task CreatePOST_WithValidModel_AddsCafeAndRedirectsToIndex()
        {
            var cafe = new Cafe { Id = 4, Name = "Cafe 4", Address = "Address 4" };

            // Act
            var result = await _controller.Create(cafe);

            // Assert
            var cafes = _context.Cafes.ToList();
            StringAssert.Contains("Cafe 4", cafes.Where(x => x.Id == 4).FirstOrDefault()?.Name);
            Assert.AreEqual(4, cafes.Count);
            Assert.IsTrue(cafes.Contains(cafe));
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Test]
        public async Task CreatePOST_WithInvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var cafe = new Cafe { Id = 4, Name = "", Address = "" };
            _controller.ModelState.AddModelError("Name", "The Name field is required.");
            _controller.ModelState.AddModelError("Address", "The Address field is required.");

            // Act
            var result = await _controller.Create(cafe);

            // Assert
            var cafes = _context.Cafes.ToList();
            Assert.AreEqual(3, cafes.Count);
            Assert.IsFalse(cafes.Contains(cafe));
            Assert.IsInstanceOf<ViewResult>(result);
            var model = ((ViewResult)result).ViewData.Model;
            Assert.IsInstanceOf<Cafe>(model);
            Assert.AreEqual(cafe, model);
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
        public async Task Edit_ReturnsNotFound_WhenCafeIsNull()
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
            var cafe = new Cafe { Id = 1, Name = "Updated Cafe", Address = "Updated Address" };

            // Act
            var result = await _controller.Edit(4, cafe);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenCafeIsNull()
        {
            // Arrange
            int id = 4;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WhenCafeIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesCafeAndRedirectsToIndex_WhenCafeIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
            Assert.IsFalse(_context.Cafes.Any(c => c.Id == id));
        }
    }
}
