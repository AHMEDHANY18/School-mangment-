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
    public class teachersController : Controller
    {
        private readonly AppDbContext _context;

        public teachersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: teachers
        public async Task<IActionResult> Index(string term, string orderby = "TeacherId", int currpage = 1)
        {


            if (!String.IsNullOrEmpty(term))
                ViewBag.term = term;
                ViewBag.orderby = orderby;

            if (ViewBag.orderby != null)

            {
                ViewBag.OrderName = orderby == "Name" ? "Name_des" : "Name";
                ViewBag.OrderEmail = orderby == "Email" ? "Email_des" : "Email";
                ViewBag.OrderUpdated_at = orderby == "Updated_at" ? "Updated_at_des" : "Updated_at";
                ViewBag.OrderCreated_at = orderby == "Created_at" ? "Created_at_des" : "Created_at";
                ViewBag.OrderStatus = orderby == "Status" ? "Status_des" : "Status";

            }
            var teachers = await _context.Teachers.ToListAsync();
            if (!string.IsNullOrEmpty(term))
            {
                teachers = teachers.Where(t => t.TeacherID.ToString().Contains(term) ||
                                               t.Name.Contains(term) ||
                                               t.Email.Contains(term) ||
                                               t.Created_at.ToString().Contains(term) ||
                                               t.Updated_at.ToString().Contains(term) ||
                                               t.Status.ToString().Contains(term)).ToList();
            }

            switch (orderby)
            {
                case "Name":
                    teachers = teachers.OrderBy(t => t.Name).ToList();
                    break;
                case "Name_des":
                    teachers = teachers.OrderByDescending(t => t.Name).ToList();
                    break;
                case "Password":
                    teachers = teachers.OrderBy(t => t.Password).ToList();
                    break;
                case "Password_des":
                    teachers = teachers.OrderByDescending(t => t.Password).ToList();
                    break;
                case "Email":
                    teachers = teachers.OrderBy(t => t.Email).ToList();
                    break;
                case "Email_des":
                    teachers = teachers.OrderByDescending(t => t.Email).ToList();
                    break;
                case "Updated_at":
                    teachers = teachers.OrderBy(t => t.Updated_at).ToList();
                    break;
                case "Updated_at_des":
                    teachers = teachers.OrderByDescending(t => t.Updated_at).ToList();
                    break;
                case "Created_at":
                    teachers = teachers.OrderBy(t => t.Created_at).ToList();
                    break;
                case "Created_at_des":
                    teachers = teachers.OrderByDescending(t => t.Created_at).ToList();
                    break;
                case "Status":
                    teachers = teachers.OrderBy(t => t.Status).ToList();
                    break;
                case "Status_des":
                    teachers = teachers.OrderByDescending(t => t.Status).ToList();
                    break;
                default:
                    teachers = teachers.OrderBy(t => t.TeacherID).ToList();
                    break;
            }
            const int pagesize = 6;
            int totalrecords = teachers.Count;

            int numofpages = (int)Math.Ceiling(Convert.ToDecimal(totalrecords / (decimal)pagesize));

            teachers = teachers.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

            ViewBag.totalrecords = totalrecords;
            ViewBag.numofpages = numofpages;
            ViewBag.currpage = currpage;
            return View(teachers);
        }

        // GET: teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherID,Name,Email,Password,Status,Created_at,Updated_at")] teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherID,Name,Email,Password,Status,Created_at,Updated_at")] teacher teacher)
        {
            if (id != teacher.TeacherID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!teacherExists(teacher.TeacherID))
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
            return View(teacher);
        }

        // GET: teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teachers == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .FirstOrDefaultAsync(m => m.TeacherID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teachers == null)
            {
                return Problem("Entity set 'AppDbContext.Teachers'  is null.");
            }
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool teacherExists(int id)
        {
          return (_context.Teachers?.Any(e => e.TeacherID == id)).GetValueOrDefault();
        }
    }
}
