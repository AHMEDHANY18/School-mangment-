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
    public class classesController : Controller
    {
        private readonly AppDbContext _context;

        public classesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: classes
        public async Task<IActionResult> Index(string term,string orderby = "classesID", int currpage = 1)
        {

            if (!String.IsNullOrEmpty(term))
                ViewBag.term = term;
            ViewBag.orderby = orderby;

            if (ViewBag.orderby != null)

            {
                ViewBag.OrderName = orderby == "Name" ? "Name_des" : "Name";
                ViewBag.OrderDescription = orderby == "Description" ? "Description_des" : "Description";
                ViewBag.OrderSchedule = orderby == "Schedule" ? "Schedule_des" : "Schedule";
                ViewBag.OrderUpdated_at = orderby == "Updated_at" ? "Updated_at_des" : "Updated_at";
                ViewBag.OrderCreated_at = orderby == "Created_at" ? "Created_at_des" : "Created_at";

            }

            var classes = await _context.Classes.ToListAsync();
            if (!string.IsNullOrEmpty(term))
            {
                classes = classes.Where(std => std.Description.Contains(term) ||
                                                    std.Updated_at.ToString().Contains(term) ||
                                                    std.Created_at.ToString().Contains(term) ||
                                                    std.Name.ToString().Contains(term) ||
                                                    std.classesID.ToString().Contains(term)).ToList();
            }

            switch (orderby)
            {
                case "Name":
                    classes = classes.OrderBy(t => t.Name).ToList();
                    break;
                case "Name_des":
                    classes = classes.OrderByDescending(t => t.Name).ToList();
                    break;
                case "Description":
                    classes = classes.OrderBy(t => t.Description).ToList();
                    break;
                case "Description_des":
                    classes = classes.OrderByDescending(t => t.Description).ToList();
                    break;
                case "Schedule":
                    classes = classes.OrderBy(t => t.Schedule).ToList();
                    break;
                case "Schedule_des":
                    classes = classes.OrderByDescending(t => t.Schedule).ToList();
                    break;
                case "Updated_at":
                    classes = classes.OrderBy(t => t.Updated_at).ToList();
                    break;
                case "Updated_at_des":
                    classes = classes.OrderByDescending(t => t.Updated_at).ToList();
                    break;
                case "Created_at":
                    classes = classes.OrderBy(t => t.Created_at).ToList();
                    break;
                case "Created_at_des":
                    classes = classes.OrderByDescending(t => t.Created_at).ToList();
                    break;
                default:
                    classes = classes.OrderBy(t => t.classesID).ToList();
                    break;

            }
            const int pagesize = 6;
            int totalrecords = classes.Count;

            int numofpages = (int)Math.Ceiling(Convert.ToDecimal(totalrecords / (decimal)pagesize));

            classes = classes.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

            ViewBag.totalrecords = totalrecords;
            ViewBag.numofpages = numofpages;
            ViewBag.currpage = currpage;

            return View(classes);
        }

        // GET: classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.classesID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // GET: classes/Create
        public IActionResult Create()
        {
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID");
            return View();
        }

        // POST: classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("classesID,TeacherID,Name,Description,Schedule,Created_at,Updated_at")] classes classes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID", classes.TeacherID);
            return View(classes);
        }

        // GET: classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes.FindAsync(id);
            if (classes == null)
            {
                return NotFound();
            }
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID", classes.TeacherID);
            return View(classes);
        }

        // POST: classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("classesID,TeacherID,Name,Description,Schedule,Created_at,Updated_at")] classes classes)
        {
            if (id != classes.classesID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!classesExists(classes.classesID))
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
            ViewData["TeacherID"] = new SelectList(_context.Teachers, "TeacherID", "TeacherID", classes.TeacherID);
            return View(classes);
        }

        // GET: classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var classes = await _context.Classes
                .Include(c => c.Teacher)
                .FirstOrDefaultAsync(m => m.classesID == id);
            if (classes == null)
            {
                return NotFound();
            }

            return View(classes);
        }

        // POST: classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'AppDbContext.Classes'  is null.");
            }
            var classes = await _context.Classes.FindAsync(id);
            if (classes != null)
            {
                _context.Classes.Remove(classes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool classesExists(int id)
        {
          return (_context.Classes?.Any(e => e.classesID == id)).GetValueOrDefault();
        }
    }
}
