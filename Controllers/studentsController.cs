using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shoolnew.Models;
using shoolnew.Helpers;

namespace shoolnew.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string term, string orderby = "StudentId", int currpage = 1)
        {
            IQueryable<student> studentsQuery = _context.Students.AsQueryable();

            // Filter by search term if provided
            if (!String.IsNullOrEmpty(term))
            {
                studentsQuery = studentsQuery.Where(std =>
                    std.FirstName.Contains(term) ||
                    std.LastName.Contains(term) ||
                    std.Email.Contains(term) ||
                    std.StudentId.ToString().Contains(term) ||
                    std.DateOfBirth.ToString().Contains(term));
            }

            // Define orderBy expressions
            Func<IQueryable<student>, IOrderedQueryable<student>> orderByFunc = orderby switch
            {
                "FirstName" => q => q.OrderBy(std => std.FirstName),
                "FirstName_des" => q => q.OrderByDescending(std => std.FirstName),
                "LastName" => q => q.OrderBy(std => std.LastName),
                "LastName_des" => q => q.OrderByDescending(std => std.LastName),
                "Email" => q => q.OrderBy(std => std.Email),
                "Email_des" => q => q.OrderByDescending(std => std.Email),
                "DateOfBirth" => q => q.OrderBy(std => std.DateOfBirth),
                "DateOfBirth_des" => q => q.OrderByDescending(std => std.DateOfBirth),
                "CreatedAt" => q => q.OrderBy(std => std.CreatedAt),
                "CreatedAt_des" => q => q.OrderByDescending(std => std.CreatedAt),
                _ => q => q.OrderBy(std => std.StudentId),
            };

            // Use PaginationHelper to get paginated result
            var paginatedResult = PaginationHelper.Paginate(studentsQuery, currpage, 6, null, orderByFunc);

            ViewBag.term = term;
            ViewBag.orderby = orderby;
            ViewBag.OrderFirstName = orderby == "FirstName" ? "FirstName_des" : "FirstName";
            ViewBag.OrderLastName = orderby == "LastName" ? "LastName_des" : "LastName";
            ViewBag.OrderEmail = orderby == "Email" ? "Email_des" : "Email";
            ViewBag.OrderDateOfBirth = orderby == "DateOfBirth" ? "DateOfBirth_des" : "DateOfBirth";
            ViewBag.OrderCreatedAt = orderby == "CreatedAt" ? "CreatedAt_des" : "CreatedAt";

            ViewBag.totalrecords = paginatedResult.TotalItems;
            ViewBag.numofpages = paginatedResult.TotalPages;
            ViewBag.currpage = paginatedResult.CurrentPage;

            return View(paginatedResult);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,Email,DateOfBirth,CreatedAt")] student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,Email,DateOfBirth,CreatedAt")] student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
