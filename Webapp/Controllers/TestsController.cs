using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebappDb;

namespace Webapp.Controllers
{
    public class TestsController : Controller
    {
        private readonly webappdbContext _context;

        public TestsController(webappdbContext context)
        {
            _context = context;
        }

        // GET: Tests
        public async Task<IActionResult> Index()
        {
            var webappdbContext = _context.Tests.Include(t => t.Experiment);
            return View(await webappdbContext.ToListAsync());
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tests = await _context.Tests
                .Include(t => t.Experiment)
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (tests == null)
            {
                return NotFound();
            }

            return View(tests);
        }

        // GET: Tests/Create
        public IActionResult Create()
        {
            ViewData["ExperimentId"] = new SelectList(_context.Experiments, "ExperimentId", "Metadata");
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestId,Metadata,ExperimentId,StartedTime,EndedTime,Name")] Tests tests)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExperimentId"] = new SelectList(_context.Experiments, "ExperimentId", "Metadata", tests.ExperimentId);
            return View(tests);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tests = await _context.Tests.FindAsync(id);
            if (tests == null)
            {
                return NotFound();
            }
            ViewData["ExperimentId"] = new SelectList(_context.Experiments, "ExperimentId", "Metadata", tests.ExperimentId);
            return View(tests);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,Metadata,ExperimentId,StartedTime,EndedTime,Name")] Tests tests)
        {
            if (id != tests.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tests);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestsExists(tests.TestId))
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
            ViewData["ExperimentId"] = new SelectList(_context.Experiments, "ExperimentId", "Metadata", tests.ExperimentId);
            return View(tests);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tests = await _context.Tests
                .Include(t => t.Experiment)
                .FirstOrDefaultAsync(m => m.TestId == id);
            if (tests == null)
            {
                return NotFound();
            }

            return View(tests);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tests = await _context.Tests.FindAsync(id);
            _context.Tests.Remove(tests);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestsExists(int id)
        {
            return _context.Tests.Any(e => e.TestId == id);
        }
    }
}
