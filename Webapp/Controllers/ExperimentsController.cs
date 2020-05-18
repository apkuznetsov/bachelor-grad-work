using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webapp.Models;
using WebappDb;

namespace Webapp.Controllers
{
    public class ExperimentsController : Controller
    {
        private readonly webappdbContext _context;

        public ExperimentsController(webappdbContext context)
        {
            _context = context;
        }


        // GET: Experiments
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Укажите IFormatProvider", Justification = "<Ожидание>")]
        public async Task<IActionResult> Index()
        {
            var currUserId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);

            // SELECT "ExperimentId", "Name" FROM experiments WHERE "ExperimentId" = Any (SELECT "ExperimentId" FROM user_experiments WHERE "UserId" = 122);
            var userExperimentsNames = _context.Experiments.
                Where(e => _context.UserExperiments.Any(ue => ue.UserId == currUserId && ue.ExperimentId == e.ExperimentId)).
                Select(e => new ExperimentNameViewModel { ExperimentId = e.ExperimentId, Name = e.Name }).ToList();

            return View(userExperimentsNames);
        }

        // GET: Experiments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiments = await _context.Experiments
                .FirstOrDefaultAsync(m => m.ExperimentId == id);
            if (experiments == null)
            {
                return NotFound();
            }

            return View(experiments);
        }

        // GET: Experiments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Experiments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExperimentId,Metadata,CreatedAt,Name")] Experiments experiments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(experiments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(experiments);
        }

        // GET: Experiments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiments = await _context.Experiments.FindAsync(id);
            if (experiments == null)
            {
                return NotFound();
            }
            return View(experiments);
        }

        // POST: Experiments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExperimentId,Metadata,CreatedAt,Name")] Experiments experiments)
        {
            if (id != experiments.ExperimentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(experiments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperimentsExists(experiments.ExperimentId))
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
            return View(experiments);
        }

        // GET: Experiments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiments = await _context.Experiments
                .FirstOrDefaultAsync(m => m.ExperimentId == id);
            if (experiments == null)
            {
                return NotFound();
            }

            return View(experiments);
        }

        // POST: Experiments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var experiments = await _context.Experiments.FindAsync(id);
            _context.Experiments.Remove(experiments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExperimentsExists(int id)
        {
            return _context.Experiments.Any(e => e.ExperimentId == id);
        }
    }
}
