using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shoolnew.Models;

namespace shoolnew.Controllers
{
    public class classe_recourceController : Controller
    {
        private readonly AppDbContext _context;

        public classe_recourceController(AppDbContext context)
        {
            _context = context;
        }

        // GET: classe_recource
        public async Task<IActionResult> Index(string term, string orderby = "ClassID", int currpage = 1)
        {


            if (!String.IsNullOrEmpty(term))
                ViewBag.term = term;
            ViewBag.orderby = orderby;

            if (ViewBag.orderby != null)

            {
                ViewBag.OrderClassID = orderby == "ClassID" ? "ClassID_des" : "ClassID";
                ViewBag.OrderName = orderby == "Name" ? "Name_des" : "Name";
                ViewBag.OrderUpdated_at = orderby == "Updated_at" ? "Updated_at_des" : "Updated_at";
                ViewBag.OrderCreated_at = orderby == "Created_at" ? "Created_at_des" : "Created_at";

            }
            var classe_recource = await _context.classe_Recources.ToListAsync();
            if (!string.IsNullOrEmpty(term))
            {
                classe_recource = classe_recource.Where(std =>

                                                     std.ClassID.ToString().Contains(term) ||
                                                      std.Updated_at.ToString().Contains(term) ||
                                                     std.Created_at.ToString().Contains(term) ||
                                                     std.classe_recourceID.ToString().Contains(term) ||
                                                     std.Name.Contains(term)).ToList();
            }


            switch (orderby)
            {
                case "Name":
                    classe_recource = classe_recource.OrderBy(t => t.Name).ToList();
                    break;
                case "Name_des":
                    classe_recource = classe_recource.OrderByDescending(t => t.Name).ToList();
                    break;
                case "ClassID":
                    classe_recource = classe_recource.OrderBy(t => t.ClassID).ToList();
                    break;
                case "ClassID_des":
                    classe_recource = classe_recource.OrderByDescending(t => t.ClassID).ToList();
                    break;
                case "Updated_at":
                    classe_recource = classe_recource.OrderBy(t => t.Updated_at).ToList();
                    break;
                case "Updated_at_des":
                    classe_recource = classe_recource.OrderByDescending(t => t.Updated_at).ToList();
                    break;
                case "Created_at":
                    classe_recource = classe_recource.OrderBy(t => t.Created_at).ToList();
                    break;
                case "Created_at_des":
                    classe_recource = classe_recource.OrderByDescending(t => t.Created_at).ToList();
                    break;
                default:
                    classe_recource = classe_recource.OrderBy(t => t.classe_recourceID).ToList();
                    break;

            }
            const int pagesize = 6;
            int totalrecords = classe_recource.Count;

            int numofpages = (int)Math.Ceiling(Convert.ToDecimal(totalrecords / (decimal)pagesize));

            classe_recource = classe_recource.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

            ViewBag.totalrecords = totalrecords;
            ViewBag.numofpages = numofpages;
            ViewBag.currpage = currpage;

            return View(classe_recource);
        }


        // GET: classe_recource/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.classe_Recources == null)
            {
                return NotFound();
            }

            var classe_recource = await _context.classe_Recources
                .FirstOrDefaultAsync(m => m.classe_recourceID == id);
            if (classe_recource == null)
            {
                return NotFound();
            }

            return View(classe_recource);
        }

        // GET: classe_recource/Create
        public IActionResult Create()
        {
            var query = _context.Classes.ToList();
            ViewBag.Classes = query;//new SelectList(query, "classesID", "Name");
            return View();
        }

        // POST: classe_recource/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("classe_recourceID,ClassID,Name,Created_at,Updated_at")] classe_recource classe_recource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classe_recource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classe_recource);
        }

        // GET: classe_recource/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.classe_Recources == null)
            {
                return NotFound();
            }

            var classe_recource = await _context.classe_Recources.FindAsync(id);
            if (classe_recource == null)
            {
                return NotFound();
            }
            return View(classe_recource);
        }

        // POST: classe_recource/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("classe_recourceID,ClassID,Name,Created_at,Updated_at")] classe_recource classe_recource)
        {
            if (id != classe_recource.classe_recourceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classe_recource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!classe_recourceExists(classe_recource.classe_recourceID))
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
            return View(classe_recource);
        }

        // GET: classe_recource/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.classe_Recources == null)
            {
                return NotFound();
            }

            var classe_recource = await _context.classe_Recources
                .FirstOrDefaultAsync(m => m.classe_recourceID == id);
            if (classe_recource == null)
            {
                return NotFound();
            }

            return View(classe_recource);
        }

        // POST: classe_recource/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.classe_Recources == null)
            {
                return Problem("Entity set 'AppDbContext.classe_Recources'  is null.");
            }
            var classe_recource = await _context.classe_Recources.FindAsync(id);
            if (classe_recource != null)
            {
                _context.classe_Recources.Remove(classe_recource);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool classe_recourceExists(int id)
        {
            return (_context.classe_Recources?.Any(e => e.classe_recourceID == id)).GetValueOrDefault();
        }
    }
}
