using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webapp.Models;
using Webapp.Models.Experiments;
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
        public IActionResult Index()
        {
            // SELECT "ExperimentId", "Name" FROM experiments WHERE "ExperimentId" = Any (SELECT "ExperimentId" FROM user_experiments WHERE "UserId" = 122);
            var userExperimentsNames = _context.Experiments.
                Where(e => _context.UserExperiments.Any(ue => ue.UserId == GetCurrUserId() && ue.ExperimentId == e.ExperimentId)).
                Select(e => new ExperimentNameViewModel { ExperimentId = e.ExperimentId, Name = e.Name }).ToList();

            return View(userExperimentsNames);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Укажите IFormatProvider", Justification = "<Ожидание>")]
        private int GetCurrUserId()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);
        }

        // GET: Experiments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExperimentDetailsViewModel experimentVm = await _context.Experiments.Select(m =>
                new ExperimentDetailsViewModel
                {
                    ExperimentId = m.ExperimentId,
                    Name = m.Name,
                    Metadata = m.Metadata,
                    CreatedAt = m.CreatedAt
                }).FirstOrDefaultAsync(m => m.ExperimentId == id).ConfigureAwait(true);

            if (experimentVm == null)
            {
                return NotFound();
            }

            return View(experimentVm);
        }

        // GET: Experiments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Experiments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public async Task<IActionResult> Create([Bind("ExperimentId,Name,Metadata")] ExperimentCreateViewModel experimentVm)
        {
            if (ModelState.IsValid)
            {
                Experiments experiment = new Experiments
                {
                    ExperimentId = experimentVm.ExperimentId,
                    Name = experimentVm.Name,
                    Metadata = experimentVm.Metadata,
                    CreatedAt = DateTimeOffset.Now
                };

                _context.Add(experiment);
                await _context.SaveChangesAsync().ConfigureAwait(true);

                UserExperiments userExperiment = new UserExperiments
                {
                    UserId = GetCurrUserId(),
                    ExperimentId = experiment.ExperimentId
                };

                _context.Add(userExperiment);
                await _context.SaveChangesAsync().ConfigureAwait(true);

                return RedirectToAction(nameof(Index));
            }

            return View(experimentVm);
        }

        // GET: Experiments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExperimentEditViewModel experimentVm = await _context.Experiments.Select(m =>
            new ExperimentEditViewModel
            {
                ExperimentId = m.ExperimentId,
                Name = m.Name,
                Metadata = m.Metadata
            }).FirstOrDefaultAsync(m => m.ExperimentId == id).ConfigureAwait(true);

            if (experimentVm == null)
            {
                return NotFound();
            }

            return View(experimentVm);
        }

        // POST: Experiments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public async Task<IActionResult> Edit(int id, [Bind("ExperimentId,Name,Metadata")] Experiments experiment)
        {
            if (id != experiment.ExperimentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(experiment);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperimentsExists(experiment.ExperimentId))
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

            return View(experiment);
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
