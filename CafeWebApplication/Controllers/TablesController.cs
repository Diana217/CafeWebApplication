﻿#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CafeWebApplication.Interfaces;

namespace CafeWebApplication.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class TablesController : Controller
    {
        private readonly DB_CafeContext _context;

        public TablesController(IDBContextFactory context)
        {
            string _connectionString = "";
            _context = context.CreateDbContext(_connectionString);
        }

        // GET: Tables
        public async Task<IActionResult> Index()
        {
            var dB_CafeContext = _context.Tables.Include(t => t.Cafe);
            return View(await dB_CafeContext.ToListAsync());
        }

        // GET: Tables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .Include(t => t.Cafe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // GET: Tables/Create
        public IActionResult Create()
        {
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name");
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CafeId,Status,Number")] Table table)
        {
            if (ModelState.IsValid)
            {
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name", table.CafeId);
            return View(table);
        }

        // GET: Tables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name", table.CafeId);
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CafeId,Status,Number")] Table table)
        {
            if (id != table.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.Id))
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
            ViewData["CafeId"] = new SelectList(_context.Cafes, "Id", "Name", table.CafeId);
            return View(table);
        }

        // GET: Tables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Tables
                .Include(t => t.Cafe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
            return _context.Tables.Any(e => e.Id == id);
        }
    }
}
