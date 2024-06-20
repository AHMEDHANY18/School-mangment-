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
    public class AssignmentSubmissionsController : Controller
    {
        private readonly AppDbContext _context;

        public AssignmentSubmissionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AssignmentSubmissions
        public async Task<IActionResult> Index(string term,string orderby = "Student", int currpage = 1)
        {
            if (!String.IsNullOrEmpty(term))
                ViewBag.term = term;
            ViewBag.orderby = orderby;

            if (ViewBag.orderby != null)

            {
                ViewBag.OrderSelectedOption = orderby == "SelectedOption" ? "SelectedOption_des" : "SelectedOption";
                ViewBag.OrderTextInput = orderby == "TextInput" ? "TextInput_des" : "TextInput";
                ViewBag.OrderAttachment = orderby == "Attachment" ? "Attachment_des" : "Attachment";
                ViewBag.OrderUpdated_at = orderby == "Updated_at" ? "Updated_at_des" : "Updated_at";
                ViewBag.OrderCreated_at = orderby == "Created_at" ? "Created_at_des" : "Created_at";

            }

            var AssignmentSubmissions = await _context.AssignmentSubmissions.ToListAsync();
            if (!string.IsNullOrEmpty(term))
            {

                AssignmentSubmissions = AssignmentSubmissions.Where(std => std.TextInput.Contains(term) ||
                                                     std.AssignmentSubmissionID.ToString().Contains(term) ||
                                                      std.Updated_at.ToString().Contains(term) ||
                                                      std.Created_at.ToString().Contains(term) ||

                                                   std.Attachment.ToString().Contains(term) ||
                                                   std.AssignmentID.ToString().Contains(term) ||
                                                   std.StudentID.ToString().Contains(term)).ToList();
            }
            switch (orderby)
            {
                case "SelectedOption":
                    AssignmentSubmissions = AssignmentSubmissions.OrderBy(t => t.SelectedOption).ToList();
                    break;
                case "SelectedOption_des":
                    AssignmentSubmissions = AssignmentSubmissions.OrderByDescending(t => t.SelectedOption).ToList();
                    break;
                case "TextInput":
                    AssignmentSubmissions = AssignmentSubmissions.OrderBy(t => t.TextInput).ToList();
                    break;
                case "TextInput_des":
                    AssignmentSubmissions = AssignmentSubmissions.OrderByDescending(t => t.TextInput).ToList();
                    break;
                case "Attachment":
                    AssignmentSubmissions = AssignmentSubmissions.OrderBy(t => t.Attachment).ToList();
                    break;
                case "Attachment_des":
                    AssignmentSubmissions = AssignmentSubmissions.OrderByDescending(t => t.Attachment).ToList();
                    break;
                case "Updated_at":
                    AssignmentSubmissions = AssignmentSubmissions.OrderBy(t => t.Updated_at).ToList();
                    break;
                case "Updated_at_des":
                    AssignmentSubmissions = AssignmentSubmissions.OrderByDescending(t => t.Updated_at).ToList();
                    break;
                case "Created_at":
                    AssignmentSubmissions = AssignmentSubmissions.OrderBy(t => t.Created_at).ToList();
                    break;
                case "Created_at_des":
                    AssignmentSubmissions = AssignmentSubmissions.OrderByDescending(t => t.Created_at).ToList();
                    break;
                default:
                    AssignmentSubmissions = AssignmentSubmissions.OrderBy(t => t.AssignmentSubmissionID).ToList();
                    break;

            }
            const int pagesize = 6;
            int totalrecords = AssignmentSubmissions.Count;

            int numofpages = (int)Math.Ceiling(Convert.ToDecimal(totalrecords / (decimal)pagesize));

            AssignmentSubmissions = AssignmentSubmissions.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

            ViewBag.totalrecords = totalrecords;
            ViewBag.numofpages = numofpages;
            ViewBag.currpage = currpage;

            return View(AssignmentSubmissions);
        }

        // GET: AssignmentSubmissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AssignmentSubmissions == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions
                .Include(a => a.Assignment)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AssignmentSubmissionID == id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }

            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Create
        public IActionResult Create()
        {
            ViewData["AssignmentID"] = new SelectList(_context.Assignments, "AssignmentID", "AssignmentID");
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: AssignmentSubmissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignmentSubmissionID,AssignmentID,StudentID,SelectedOption,TextInput,Attachment,Created_at,Updated_at")] AssignmentSubmission assignmentSubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignmentSubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignmentID"] = new SelectList(_context.Assignments, "AssignmentID", "AssignmentID", assignmentSubmission.AssignmentID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId", assignmentSubmission.StudentID);
            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AssignmentSubmissions == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions.FindAsync(id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }
            ViewData["AssignmentID"] = new SelectList(_context.Assignments, "AssignmentID", "AssignmentID", assignmentSubmission.AssignmentID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId", assignmentSubmission.StudentID);
            return View(assignmentSubmission);
        }

        // POST: AssignmentSubmissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignmentSubmissionID,AssignmentID,StudentID,SelectedOption,TextInput,Attachment,Created_at,Updated_at")] AssignmentSubmission assignmentSubmission)
        {
            if (id != assignmentSubmission.AssignmentSubmissionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentSubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentSubmissionExists(assignmentSubmission.AssignmentSubmissionID))
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
            ViewData["AssignmentID"] = new SelectList(_context.Assignments, "AssignmentID", "AssignmentID", assignmentSubmission.AssignmentID);
            ViewData["StudentID"] = new SelectList(_context.Students, "StudentId", "StudentId", assignmentSubmission.StudentID);
            return View(assignmentSubmission);
        }

        // GET: AssignmentSubmissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AssignmentSubmissions == null)
            {
                return NotFound();
            }

            var assignmentSubmission = await _context.AssignmentSubmissions
                .Include(a => a.Assignment)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.AssignmentSubmissionID == id);
            if (assignmentSubmission == null)
            {
                return NotFound();
            }

            return View(assignmentSubmission);
        }

        // POST: AssignmentSubmissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AssignmentSubmissions == null)
            {
                return Problem("Entity set 'AppDbContext.AssignmentSubmissions'  is null.");
            }
            var assignmentSubmission = await _context.AssignmentSubmissions.FindAsync(id);
            if (assignmentSubmission != null)
            {
                _context.AssignmentSubmissions.Remove(assignmentSubmission);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentSubmissionExists(int id)
        {
          return (_context.AssignmentSubmissions?.Any(e => e.AssignmentSubmissionID == id)).GetValueOrDefault();
        }
    }
}
