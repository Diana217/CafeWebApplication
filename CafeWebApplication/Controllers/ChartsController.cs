using Microsoft.AspNetCore.Mvc;

namespace CafeWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DB_CafeContext _context;
        public ChartsController(DB_CafeContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var types = _context.ItemTypes.ToList();
            List<object> typMenu = new List<object>();
            typMenu.Add(new[] { "Тип позиції меню", "Кількість позицій меню" });
            foreach(var t in types)
            {
                var menu = _context.MenuItems.Where(menu => menu.ItemTypeId == t.Id).ToList();
                typMenu.Add(new object[] {t.Type, menu.Count()});
            }
            return new JsonResult(typMenu);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var cafes = _context.Cafes.ToList();
            List<object> cafEmployee = new List<object>();
            cafEmployee.Add(new[] { "Кав'ярня", "Кількість працівників" });
            foreach (var c in cafes)
            {
                var employee = _context.Employees.Where(emp => emp.CafeId == c.Id).ToList();
                cafEmployee.Add(new object[] { c.Name, employee.Count() });
            }
            return new JsonResult(cafEmployee);
        }

        [HttpGet("JsonData2")]
        public JsonResult JsonData2()
        {
            var menuItems = _context.MenuItems.ToList();
            List<object> menuOrder = new List<object>();
            menuOrder.Add(new[] { "Позиція меню", "Кількість замовлень" });
            foreach (var m in menuItems)
            {
                var order = _context.MenuOrders.Where(ord => ord.MenuItemId == m.Id).ToList();
                if(order.Count() > 0)
                {
                    menuOrder.Add(new object[] { m.Name, order.Count() });
                }
            }
            return new JsonResult(menuOrder);
        }
    }
}
