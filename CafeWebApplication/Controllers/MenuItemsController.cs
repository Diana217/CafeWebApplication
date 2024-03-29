﻿#nullable disable
using CafeWebApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeWebApplication.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly DB_CafeContext _context;

        public MenuItemsController(IDBContextFactory context)
        {
            string _connectionString = "";
            _context = context.CreateDbContext(_connectionString);
        }

        // GET: MenuItems
        public async Task<IActionResult> Index(int? id, string? type)
        {
            if (id == null) return RedirectToAction("ItemTypes", "Index");
            //знаходження позицій меню за типом
            ViewBag.ItemTypeId = id;
            ViewBag.ItemType = type;

            //var dB_CafeContext = _context.MenuItems.Include(m => m.Cafe).Include(m => m.ItemType);
            //return View(await dB_CafeContext.ToListAsync());

            var menuItemsByType = _context.MenuItems.Where(m => m.ItemTypeId == id).Include(m => m.ItemType).Include(m => m.Cafe);
            return View(await menuItemsByType.ToListAsync());
        }

        // GET: MenuItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Cafe)
                .Include(m => m.ItemType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        public IActionResult Create(int itemTypeId)
        {
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name"); 
            //ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Type");
            ViewBag.ItemTypeId = itemTypeId;
            ViewBag.ItemType = _context.ItemTypes.Where(c => c.Id == itemTypeId).FirstOrDefault().Type;
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int itemTypeId, [Bind("Id,CafeId,ItemTypeId,Name,Price,Status")] MenuItem menuItem)
        {
            menuItem.ItemTypeId = itemTypeId;
            if (ModelState.IsValid)
            {
                _context.Add(menuItem);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "MenuItems", new { id = itemTypeId, name = _context.ItemTypes.Where(c => c.Id == itemTypeId).FirstOrDefault().Type });
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name", menuItem.CafeId); 
            //ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Type", menuItem.ItemTypeId);
            //return View(menuItem);
            return RedirectToAction("Index", "MenuItems", new { id = itemTypeId, name = _context.ItemTypes.Where(c => c.Id == itemTypeId).FirstOrDefault().Type });
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name", menuItem.CafeId);

            //ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Type", menuItem.ItemTypeId);
            ViewBag.ItemTypeId = menuItem.ItemTypeId;
            ViewBag.ItemType = _context.ItemTypes.Where(c => c.Id == menuItem.ItemTypeId).FirstOrDefault().Type;

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CafeId,ItemTypeId,Name,Price,Status")] MenuItem menuItem)
        {
            if (id != menuItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(menuItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "MenuItems", new { id = menuItem.ItemTypeId, name = _context.ItemTypes.Where(c => c.Id == menuItem.ItemTypeId).FirstOrDefault().Type });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name", menuItem.CafeId);
            //ViewData["ItemTypeId"] = new SelectList(_context.ItemTypes, "Id", "Type", menuItem.ItemTypeId);

            return RedirectToAction("Index", "MenuItems", new { id = menuItem.ItemTypeId, name = _context.ItemTypes.Where(c => c.Id == menuItem.ItemTypeId).FirstOrDefault().Type });
            //return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var menuItem = await _context.MenuItems
                 .Include(m => m.Cafe)
                 .Include(m => m.ItemType)
                 .FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "MenuItems", new { id = menuItem.ItemTypeId, name = _context.ItemTypes.Where(c => c.Id == menuItem.ItemTypeId).FirstOrDefault().Type });
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}
