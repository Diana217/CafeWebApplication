using CafeWebApplication.Controllers;
using CafeWebApplication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using CafeWebApplication.Interfaces;

namespace NUnit
{
    [TestFixture]
    [Category("CafeTests")]
    public class EmployeesControllerTests
    {
        private DB_CafeContext _context;
        private Mock<IDBContextFactory> _contextFactoryMock;
        private EmployeesController _controller;
        
        [SetUp]
        public void SetUp()
        {
            SeedDatabase seed = new SeedDatabase();
            seed.SeedDB();

            _context = seed._context;
            _contextFactoryMock = seed._contextFactoryMock;

            _controller = new EmployeesController(_contextFactoryMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfEmployees()
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
        public async Task Details_ReturnsNotFoundResult_WhenEmployeeIsNotFound()
        {
            // Act
            var result = await _controller.Details(4);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Details_ReturnsViewResult_WithEmployee()
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
            var employee = new Employee
            {
                CafeId = 1,
                EmployeeType = true,
                Name = "John Doe",
                PhoneNumber = "+380685577898",
                DateOfEmployment = DateTime.Now,
                DateOfRelease = null
            };

            // Act
            var result = await _controller.Create(employee) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Create_Post_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var employee = new Employee
            {
                CafeId = 1,
                EmployeeType = true,
                Name = "", // Required field
                PhoneNumber = "+380685577898",
                DateOfEmployment = DateTime.Now,
                DateOfRelease = null
            };

            _controller.ModelState.AddModelError("Name", "Name is required.");

            // Act
            var result = await _controller.Create(employee) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee, result.Model);
        }

        [Test]
        public async Task Edit_Post_ValidModelState_ReturnsRedirectToActionResult()
        {
            // Arrange
            var employee = await _context.Employees.FirstOrDefaultAsync();

            employee.Name = "Jane Doe";

            // Act
            var result = await _controller.Edit(employee.Id, employee) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [Test]
        public async Task Edit_Post_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var employee = await _context.Employees.FirstOrDefaultAsync();

            employee.Name = ""; // Required field

            _controller.ModelState.AddModelError("Name", "Name is required.");

            // Act
            var result = await _controller.Edit(employee.Id, employee) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(employee, result.Model);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenEmployeeIsNull()
        {
            // Arrange
            int id = 4;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsViewResult_WhenEmployeeIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task DeleteConfirmed_RemovesEmployeeAndRedirectsToIndex_WhenEmployeeIsNotNull()
        {
            // Arrange
            int id = 1;

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.AreEqual("Index", (result as RedirectToActionResult).ActionName);
            Assert.IsFalse(_context.Employees.Any(c => c.Id == id));
        }
    }
}
