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
    public class AssignmentsController : Controller
    {
        private readonly AppDbContext _context;

        public AssignmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Assignments
        public async Task<IActionResult> Index(string term,string orderby = "AssignmentID" ,int currpage = 1)
        {
            if (!String.IsNullOrEmpty(term))
                ViewBag.term = term;
            ViewBag.orderby = orderby;

            if (ViewBag.orderby != null)

            {
                ViewBag.OrderQuestion = orderby == "Question" ? "Question_des" : "Question";
                ViewBag.OrderOption = orderby == "Option" ? "Option_des" : "Option";
                ViewBag.OrderDeadline = orderby == "Deadline" ? "Deadline_des" : "Deadline";
                ViewBag.OrderAttachment = orderby == "Attachment" ? "Attachment_des" : "Attachment";
                ViewBag.OrderCreated_at = orderby == "Created_at" ? "Created_at_des" : "Created_at";
                ViewBag.OrderUpdated_at = orderby == "Updated_at" ? "Updated_at_des" : "Updated_at";

            }


            var Assignments = await _context.Assignments.ToListAsync();
            if (!string.IsNullOrEmpty(term))
            {
                Assignments = Assignments.Where(std => std.Question.Contains(term) ||
                                                    std.Updated_at.ToString().Contains(term) ||
                                                    std.Created_at.ToString().Contains(term) ||
                                                    std.AssignmentID.ToString().Contains(term) ||
                                                    std.Attachment.ToString().Contains(term) ||
                                                    std.ClassID.ToString().Contains(term) ||
                                                    std.Question.ToString().Contains(term) ||
                                                    std.Deadline.ToString().Contains(term)).ToList();
            }

            switch (orderby)
            {
                case "Question":
                    Assignments = Assignments.OrderBy(t => t.Question).ToList();
                    break;
                case "Question_des":
                    Assignments = Assignments.OrderByDescending(t => t.Question).ToList();
                    break;
                case "Option":
                    Assignments = Assignments.OrderBy(t => t.Option).ToList();
                    break;
                case "Option_des":
                    Assignments = Assignments.OrderByDescending(t => t.Option).ToList();
                    break;
                case "Attachment":
                    Assignments = Assignments.OrderBy(t => t.Attachment).ToList();
                    break;
                case "Attachment_des":
                    Assignments = Assignments.OrderByDescending(t => t.Attachment).ToList();
                    break;
                case "Updated_at":
                    Assignments = Assignments.OrderBy(t => t.Updated_at).ToList();
                    break;
                case "Updated_at_des":
                    Assignments = Assignments.OrderByDescending(t => t.Updated_at).ToList();
                    break;
                case "Created_at":
                    Assignments = Assignments.OrderBy(t => t.Created_at).ToList();
                    break;
                case "Created_at_des":
                    Assignments = Assignments.OrderByDescending(t => t.Created_at).ToList();
                    break;
                case "Deadline":
                    Assignments = Assignments.OrderBy(t => t.Deadline).ToList();
                    break;
                case "Deadline_des":
                    Assignments = Assignments.OrderByDescending(t => t.Deadline).ToList();
                    break;
                default:
                    Assignments = Assignments.OrderBy(t => t.AssignmentID).ToList();
                    break;

            }
            const int pagesize = 6;
            int totalrecords = Assignments.Count;

            int numofpages = (int)Math.Ceiling(Convert.ToDecimal(totalrecords / (decimal)pagesize));

            Assignments = Assignments.Skip((currpage - 1) * pagesize).Take(pagesize).ToList();

            ViewBag.totalrecords = totalrecords;
            ViewBag.numofpages = numofpages;
            ViewBag.currpage = currpage;

            return View(Assignments);
        }

        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .FirstOrDefaultAsync(m => m.AssignmentID == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        public IActionResult Create()
        {
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignmentID,ClassID,Question,Option,Deadline,Attachment,Created_at,Updated_at")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID", assignment.ClassID);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID", assignment.ClassID);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignmentID,ClassID,Question,Option,Deadline,Attachment,Created_at,Updated_at")] Assignment assignment)
        {
            if (id != assignment.AssignmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.AssignmentID))
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
            ViewData["ClassID"] = new SelectList(_context.Classes, "classesID", "classesID", assignment.ClassID);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Assignments == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.Class)
                .FirstOrDefaultAsync(m => m.AssignmentID == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Assignments == null)
            {
                return Problem("Entity set 'AppDbContext.Assignments'  is null.");
            }
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
          return (_context.Assignments?.Any(e => e.AssignmentID == id)).GetValueOrDefault();
        }
    }
}
