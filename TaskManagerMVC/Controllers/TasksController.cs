using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagerMVC.Data;
using TaskManagerMVC.Models;

namespace TaskManagerMVC.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskContext _context;
        private readonly ILogger _logger;

        public TasksController(TaskContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Tasks
        // Displays all tasks in the database. The method gets a list of tasks from the Tasks entity set 
        // by reading the Tasks property of the database context instance.
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["DescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CompletedSortParm"] = sortOrder == "completed_desc" ? "description_desc" : "completed_desc";
            ViewData["CurrentFilter"] = searchString;

            var tasks = from s in _context.Tasks
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "completed_asc":
                    tasks = tasks.OrderBy(t => t.CompletionStatus);
                    break;
                case "completed_desc":
                    tasks = tasks.OrderByDescending(t => t.CompletionStatus);
                    break;
                case "description_desc":
                    tasks = tasks.OrderByDescending(t => t.Description);
                    break;
                case "Date":
                    tasks = tasks.OrderBy(t => t.DueDate);
                    break;
                case "date_desc":
                    tasks = tasks.OrderByDescending(t => t.DueDate);
                    break;
                default:
                    tasks = tasks.OrderBy(t => t.Description);
                    break;
            }
            return View(await tasks.AsNoTracking().ToListAsync());
        }


        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Description,CompletionStatus,DueDate")] Models.Task task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(task);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating the user's task.");      
                ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Description,CompletionStatus,DueDate")] Models.Task task)
        {
            if (id != task.TaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.TaskId))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
