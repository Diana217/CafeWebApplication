#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeWebApplication;

namespace CafeWebApplication.Controllers
{
    public class MenuOrdersController : Controller
    {
        private readonly DB_CafeContext _context;

        public MenuOrdersController(DB_CafeContext context)
        {
            _context = context;
        }

        // GET: MenuOrders
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null) return RedirectToAction("Orders", "Index");
            //знаходження замовлень з меню за замовленнями
            ViewBag.OrderId = id;

            var dB_CafeContext = _context.MenuOrders.Where(m => m.OrderId == id).Include(m => m.Order).Include(m => m.MenuItem);
            //var dB_CafeContext = _context.MenuOrders.Include(m => m.MenuItem).Include(m => m.Order);
            return View(await dB_CafeContext.ToListAsync());
        }

        // GET: MenuOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOrder = await _context.MenuOrders
                .Include(m => m.MenuItem)
                .Include(m => m.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuOrder == null)
            {
                return NotFound();
            }

            return View(menuOrder);
        }

        // GET: MenuOrders/Create
        public IActionResult Create(int orderId)
        {
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name");
            //ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewBag.OrderId = orderId;
            return View();
        }

        // POST: MenuOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int orderId, [Bind("Id,MenuItemId,OrderId,Amount")] MenuOrder menuOrder)
        {
            menuOrder.OrderId = orderId;
            if (ModelState.IsValid)
            {
                _context.Add(menuOrder);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "MenuOrders", new { id = orderId });
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", menuOrder.MenuItemId);
            //ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", menuOrder.OrderId);
            return RedirectToAction("Index", "MenuOrders", new { id = orderId });
            //return View(menuOrder);
        }

        // GET: MenuOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOrder = await _context.MenuOrders.FindAsync(id);
            if (menuOrder == null)
            {
                return NotFound();
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", menuOrder.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", menuOrder.OrderId);
            return View(menuOrder);
        }

        // POST: MenuOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuItemId,OrderId,Amount")] MenuOrder menuOrder)
        {
            if (id != menuOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuOrderExists(menuOrder.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", menuOrder.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", menuOrder.OrderId);
            return View(menuOrder);
        }

        // GET: MenuOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuOrder = await _context.MenuOrders
                .Include(m => m.MenuItem)
                .Include(m => m.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuOrder == null)
            {
                return NotFound();
            }

            return View(menuOrder);
        }

        // POST: MenuOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuOrder = await _context.MenuOrders.FindAsync(id);
            _context.MenuOrders.Remove(menuOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuOrderExists(int id)
        {
            return _context.MenuOrders.Any(e => e.Id == id);
        }
    }
}
