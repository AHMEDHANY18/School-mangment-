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
    public class classStudentsController : Controller
    {
        private readonly AppDbContext _context;

        public classStudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: classStudents
        public async Task<IActionResult> Index(string term, string orderby = "StudentID")
        {
            if (!String.IsNullOrEmpty(term))
                ViewBag.term = term;
            ViewBag.orderby = orderby;

            if (ViewBag.orderby != null)

            {
                ViewBag.OrderVisitCount = orderby == "VisitCount" ? "VisitCount_des" : "VisitCount";
                ViewBag.OrderClassTime = orderby == "ClassTime" ? "ClassTime_des" : "ClassTime";
                ViewBag.OrderUpdated_at = orderby == "Updated_at" ? "Updated_at_des" : "Updated_at";
                ViewBag.OrderCreated_at = orderby == "Created_at" ? "Created_at_des" : "Created_at";

            }

            var classStudents = await _context.ClassStudents.ToListAsync();
            if (!string.IsNullOrEmpty(term))
            {

                classStudents = classStudents.Where(std =>

                                                   std.ClassID.ToString().Contains(term) ||
                                                    std.ClassTime.ToShortDateString().Contains(term) ||
                                                   std.Created_at.ToString().Contains(term) ||
                                                   std.Updated_at.ToString().Contains(term) ||
                                                   std.VisitCount.ToString().Contains(term) ||
                                                   std.ClassStudentId.ToString().Contains(term) ||
                                                   std.StudentID.ToString().Contains(term)).ToList();
            }

            switch (orderby)
            {
                case "VisitCount":
                    classStudents = classStudents.OrderBy(t => t.VisitCount).ToList();
                    break;
                case "VisitCount_des":
                    classStudents = classStudents.OrderByDescending(t => t.VisitCount).ToList();
                    break;
                case "ClassTime":
                    classStudents = classStudents.OrderBy(t => t.ClassTime).ToList();
                    break;
                case "ClassTime_des":
                    classStudents = classStudents.OrderByDescending(t => t.ClassTime).ToList();
                    break;
               
                case "Updated_at":
                    classStudents = classStudents.OrderBy(t => t.Updated_at).ToList();
                    break;
                case "Updated_at_des":
                    classStudents = classStudents.OrderByDescending(t => t.Updated_at).ToList();
                    break;
                case "Created_at":
                    classStudents = classStudents.OrderBy(t => t.Created_at).ToList();
                    break;
                case "Created_at_des":
                    classStudents = classStudents.OrderByDescending(t => t.Created_at).ToList();
                    break;
                default:
                    classStudents = classStudents.OrderBy(t => t.ClassStudentId).ToList();
                    break;
            }

            return View(classStudents);
        }

        // GET: classStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClassStudents == null)
            {
                return NotFound();
            }

            var classStudent = await _context.ClassStudents
                .Include(c => c.Class)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.ClassStudentId == id);
            if (classStudent == null)
            {
                return NotFound();
            }

            return View(classStudent);
        }

        // GET: classStudents/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: classStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassStudentId,ClassID,StudentID,VisitCount,ClassTime,Created_at,Updated_at")] classStudent classStudent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classStudent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID", classStudent.ClassID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId", classStudent.StudentID);
            return View(classStudent);
        }

        // GET: classStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClassStudents == null)
            {
                return NotFound();
            }

            var classStudent = await _context.ClassStudents.FindAsync(id);
            if (classStudent == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID", classStudent.ClassID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId", classStudent.StudentID);
            return View(classStudent);
        }

        // POST: classStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClassStudentId,ClassID,StudentID,VisitCount,ClassTime,Created_at,Updated_at")] classStudent classStudent)
        {
            if (id != classStudent.ClassStudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!classStudentExists(classStudent.ClassStudentId))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID", classStudent.ClassID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId", classStudent.StudentID);
            return View(classStudent);
        }

        // GET: classStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClassStudents == null)
            {
                return NotFound();
            }

            var classStudent = await _context.ClassStudents
                .Include(c => c.Class)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.ClassStudentId == id);
            if (classStudent == null)
            {
                return NotFound();
            }

            return View(classStudent);
        }

        // POST: classStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClassStudents == null)
            {
                return Problem("Entity set 'AppDbContext.ClassStudents'  is null.");
            }
            var classStudent = await _context.ClassStudents.FindAsync(id);
            if (classStudent != null)
            {
                _context.ClassStudents.Remove(classStudent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool classStudentExists(int id)
        {
          return (_context.ClassStudents?.Any(e => e.ClassStudentId == id)).GetValueOrDefault();
        }
    }
}
