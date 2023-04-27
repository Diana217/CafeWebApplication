using CafeWebApplication;
using Microsoft.EntityFrameworkCore;

namespace NUnit
{
    internal class SeedDatabase
    {
        public DB_CafeContext _context;
        public SeedDatabase()
        {
            var options = new DbContextOptionsBuilder<DB_CafeContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;
            _context = new DB_CafeContext(options);
        }

        public void SeedDB()
        {
            var cafes = new List<Cafe>
            {
                new Cafe {Id = 1, Name = "Cafe 1", Address = "Address 1"},
                new Cafe {Id = 2, Name = "Cafe 2", Address = "Address 2"},
                new Cafe {Id = 3, Name = "Cafe 3", Address = "Address 3"}
            };
            _context.Cafes.AddRange(cafes);

            var itemTypes = new List<ItemType>
            {
                new ItemType {Id = 1, Type = "Dessert"},
                new ItemType {Id = 2, Type = "Drink"},
                new ItemType {Id = 3, Type = "Salad"}
            };
            _context.ItemTypes.AddRange(itemTypes);

            var menuItems = new List<MenuItem>
            {
                new MenuItem {Id = 1, CafeId = 1, ItemTypeId = 1, Name = "Dessert 1", Price = 75, Status = true},
                new MenuItem {Id = 2, CafeId = 2, ItemTypeId = 2, Name = "Drink 1", Price = 50, Status = false},
                new MenuItem {Id = 3, CafeId = 3, ItemTypeId = 1, Name = "Dessert 2", Price = 60, Status = true}
            };
            _context.MenuItems.AddRange(menuItems);

            var tables = new List<Table>
            {
                new Table {Id = 1, CafeId = 1, Status = true, Number = 1 },
                new Table {Id = 2, CafeId = 2, Status = false, Number = 2 },
                new Table {Id = 3, CafeId = 3, Status = false, Number = 1 }
            };
            _context.Tables.AddRange(tables);

            var employees = new List<Employee>
            {
                new Employee {Id = 1, CafeId = 1, Name = "Employee 1", EmployeeType = true, PhoneNumber = "+380985588741", DateOfEmployment = DateTime.Now },
                new Employee {Id = 2, CafeId = 2, Name = "Employee 2", EmployeeType = true, PhoneNumber = "+380985999741", DateOfEmployment = DateTime.Now },
                new Employee {Id = 3, CafeId = 2, Name = "Employee 3", EmployeeType = false, PhoneNumber = "+380985741741", DateOfEmployment = DateTime.Now }
            };
            _context.Employees.AddRange(employees);

            var orders = new List<Order>
            {
                new Order {Id = 1, WaiterId = 3, TableId = 1, Date = DateTime.Now },
                new Order {Id = 2, WaiterId = 3, TableId = 2, Date = DateTime.Now },
                new Order {Id = 3, WaiterId = 3, TableId = 3, Date = DateTime.Now }
            };
            _context.Orders.AddRange(orders);

            var menuOrders = new List<MenuOrder>
            {
                new MenuOrder {Id = 1, MenuItemId = 1, OrderId = 1, Amount = 2 },
                new MenuOrder {Id = 2, MenuItemId = 2, OrderId = 2, Amount = 1 },
                new MenuOrder {Id = 3, MenuItemId = 3, OrderId = 3, Amount = 1 }
            };
            _context.MenuOrders.AddRange(menuOrders);

            _context.SaveChanges();
        }
    }
}
