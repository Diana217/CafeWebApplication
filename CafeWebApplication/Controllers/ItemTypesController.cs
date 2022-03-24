#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeWebApplication;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace CafeWebApplication.Controllers
{
    public class ItemTypesController : Controller
    {
        private readonly DB_CafeContext _context;

        public ItemTypesController(DB_CafeContext context)
        {
            _context = context;
        }

        // GET: ItemTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemTypes.ToListAsync());
        }

        // GET: ItemTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            //return View(itemType);
            return RedirectToAction("Index", "MenuItems", new { id = itemType.Id, name = itemType.Type});
        }

        // GET: ItemTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemType);
        }

        // GET: ItemTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes.FindAsync(id);
            if (itemType == null)
            {
                return NotFound();
            }
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type")] ItemType itemType)
        {
            if (id != itemType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemTypeExists(itemType.Id))
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
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemType = await _context.ItemTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemType == null)
            {
                return NotFound();
            }

            return View(itemType);
        }

        // POST: ItemTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemType = await _context.ItemTypes.FindAsync(id);
            _context.ItemTypes.Remove(itemType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemTypeExists(int id)
        {
            return _context.ItemTypes.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                ItemType newtype;
                                var t = (from typ in _context.ItemTypes
                                         where typ.Type.Contains(worksheet.Name)
                                         select typ).ToList();
                                if (t.Count > 0)
                                {
                                    newtype = t[0];
                                }
                                else
                                {
                                    newtype = new ItemType();
                                    newtype.Type = worksheet.Name;
                                    _context.ItemTypes.Add(newtype);
                                }                 
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        MenuItem menuItem = new MenuItem();
                                        menuItem.Name = row.Cell(1).Value.ToString();
                                        menuItem.Price = Convert.ToInt32(row.Cell(2).Value);
                                        menuItem.Status = Convert.ToBoolean(row.Cell(3).Value);
                                        menuItem.ItemType = newtype;
                                        _context.MenuItems.Add(menuItem);
                                        if (row.Cell(4).Value.ToString().Length > 0)
                                        {
                                            Cafe cafe;

                                            var c = (from caf in _context.Cafes
                                                     where caf.Name.Contains(row.Cell(4).Value.ToString())
                                                     select caf).ToList();
                                            if (c.Count > 0)
                                            {
                                                cafe = c[0];
                                            }
                                            else
                                            {
                                                cafe = new Cafe();
                                                cafe.Name = row.Cell(4).Value.ToString();
                                                cafe.Address = "from EXCEL";
                                                _context.Cafes.Add(cafe);
                                            }
                                            menuItem.Cafe = cafe;
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var types = _context.ItemTypes.Include("MenuItems").ToList();

                foreach (var t in types)
                {
                    var worksheet = workbook.Worksheets.Add(t.Type);
                    worksheet.Cell("A1").Value = "Назва";
                    worksheet.Cell("B1").Value = "Ціна";
                    worksheet.Cell("C1").Value = "Статус";
                    worksheet.Cell("D1").Value = "Кав'ярня";

                    worksheet.Row(1).Style.Font.Bold = true;
                    worksheet.Column(1).Width = 22;
                    worksheet.Column(4).Width = 19;

                    var menuItems = t.MenuItems.ToList();

                    for (int i = 0; i < menuItems.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = menuItems[i].Name;
                        worksheet.Cell(i + 2, 2).Value = menuItems[i].Price;
                        worksheet.Cell(i + 2, 3).Value = menuItems[i].Status;

                        var cafe = _context.Cafes.Where(c => c.Id == menuItems[i].CafeId).ToList();
                        foreach(var caf in cafe)
                        {
                            worksheet.Cell(i + 2, 4).Value = caf.Name;
                        }
                    }
                }
                using (var stream = new MemoryStream())

                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"кав'ярня_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

        public ActionResult DocExport()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (WordprocessingDocument package = WordprocessingDocument.Create(ms, WordprocessingDocumentType.Document))
                {
                    var menuItems = _context.MenuItems.ToList();

                    MainDocumentPart mainPart = package.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    var body = new Body();

                    foreach (var item in menuItems)
                    {
                        #region RUNs
                        Run runName = new Run();
                        Run runPrice = new Run();
                        Run runStatus = new Run();
                        Run runType = new Run();
                        Run runCafe = new Run();

                        Run run = new Run();
                        #endregion RUNs

                        #region PARAGRAPHs
                        Paragraph itemName = new Paragraph();
                        Paragraph itemPrice = new Paragraph();
                        Paragraph itemStatus = new Paragraph();
                        Paragraph itemType = new Paragraph();
                        Paragraph itemCafe = new Paragraph();

                        Paragraph paragraph = new Paragraph();
                        #endregion PARAGRAPHs

                        var type = _context.ItemTypes.Where(t => t.Id == item.ItemTypeId).ToList();
                        var cafe = _context.Cafes.Where(c => c.Id == item.CafeId).ToList();

                        RunProperties runHeaderProperties = runName.AppendChild(new RunProperties(new Bold()));

                        runName.AppendChild(new Text($"Назва:    		                {item.Name}"));
                        itemName.Append(runName);
                        body.Append(itemName);

                        runPrice.AppendChild(new Text($"Ціна:    		                   {Math.Round(item.Price)} грн."));
                        itemPrice.Append(runPrice);
                        body.Append(itemPrice);

                        runStatus.AppendChild(new Text($"Статус:    		                {item.Status}"));
                        itemStatus.Append(runStatus);
                        body.Append(itemStatus);

                        if(type.Count() > 0)
                        {
                            foreach(var typ in type)
                            {
                                runType.AppendChild(new Text($"Тип:                           {typ.Type}"));
                                itemType.Append(runType);
                                body.Append(itemType);
                            }
                        }

                        if(cafe.Count() > 0)
                        {
                            foreach(var caf in cafe)
                            {
                                runCafe.AppendChild(new Text($"Кав'ярня:                 {caf.Name}"));
                                itemCafe.Append(runCafe);
                                body.Append(itemCafe);
                            }
                        }

                        run.AppendChild(new Text("***************************************************************************************"));
                        paragraph.Append(run);
                        body.Append(paragraph);
                    }
                    mainPart.Document.Append(body);
                    package.Close();
                }
                ms.Flush();
                return new FileContentResult(ms.ToArray(), "application/vnd.ms-word")
                {
                    FileDownloadName = $"кав'ярня_{DateTime.UtcNow.ToShortDateString()}.docx"
                };
            }
        }
    }
}
