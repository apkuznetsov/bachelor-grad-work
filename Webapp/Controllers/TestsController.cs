using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;
using Webapp.Models;
using Webapp.Models.Tests;
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
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Experiments");
        }

        // GET: Tests/Experiment/5
        public IActionResult Experiment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!_context.UserExperiments.Any(ue => ue.UserId == GetCurrUserId() && ue.ExperimentId == id))
            {
                return NotFound();
            }

            var userExperimentTestsNames = _context.Tests.
                Where(t => t.ExperimentId == id
                && _context.UserExperiments.Any(ue => ue.UserId == GetCurrUserId() && ue.ExperimentId == t.ExperimentId)).
                Select(t => new TestNameViewModel { TestId = t.TestId, Name = t.Name }).ToList();

            if (userExperimentTestsNames == null)
            {
                return NotFound();
            }

            ViewBag.ExperimentName = _context.Experiments.FirstOrDefault(e => e.ExperimentId == id).Name;
            ViewBag.ExperimentId = id;

            return View(userExperimentTestsNames);
        }

        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testVm = await _context.Tests.
                Select(m =>
                new TestDetailsViewModel
                {
                    TestId = m.TestId,
                    Name = m.Name,
                    Metadata = m.Metadata,
                    StartedTime = m.StartedTime,
                    EndedTime = m.EndedTime,
                    ExperimentId = m.ExperimentId
                }).
                FirstOrDefaultAsync(m => m.TestId == id).ConfigureAwait(true);

            if (testVm == null)
            {
                return NotFound();
            }

            if (!DoesUserHaveAccess(testVm.ExperimentId))
            {
                return NotFound();
            }

            testVm.ExperimentName = _context.Experiments.FirstOrDefault(m => m.ExperimentId == testVm.ExperimentId).Name;
            return View(testVm);
        }

        // GET: Tests/Create/ExperimentId
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Укажите IFormatProvider", Justification = "<Ожидание>")]
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!DoesUserHaveAccess(Convert.ToInt32(id)))
            {
                return NotFound();
            }

            var experimentVm = _context.Experiments.
                Select(m =>
                new ExperimentNameViewModel
                {
                    ExperimentId = m.ExperimentId,
                    Name = m.Name
                }).
                FirstOrDefault(m => m.ExperimentId == id);

            TestCreateViewModel testVm = new TestCreateViewModel
            {
                ExperimentId = experimentVm.ExperimentId,
                ExperimentName = experimentVm.Name
            };

            return View(testVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public async Task<IActionResult> Create([Bind("Name,Metadata,ExperimentId,ExperimentName")] TestCreateViewModel testVm)
        {
            if (!DoesUserHaveAccess(testVm.ExperimentId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Tests test = new Tests
                {
                    Name = testVm.Name,
                    Metadata = testVm.Metadata,
                    ExperimentId = testVm.ExperimentId,
                    StartedTime = DateTime.Now
                };
                _context.Add(test);
                await _context.SaveChangesAsync().ConfigureAwait(true);

                return RedirectToAction(nameof(Index));
            }

            return View(testVm);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TestEditViewModel testVm = await _context.Tests.Select(m =>
            new TestEditViewModel
            {
                TestId = m.TestId,
                Name = m.Name,
                Metadata = m.Metadata,
                StartedTime = m.StartedTime,
                EndedTime = m.EndedTime,
                ExperimentId = m.ExperimentId
            }).FirstOrDefaultAsync(m => m.TestId == id).ConfigureAwait(true);

            if (testVm == null)
            {
                return NotFound();
            }

            if (!DoesUserHaveAccess(testVm.ExperimentId))
            {
                return NotFound();
            }

            testVm.ExperimentName = _context.Experiments.FirstOrDefault(m => m.ExperimentId == testVm.ExperimentId).Name;
            return View(testVm);
        }

        // POST: Tests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,Name,Metadata,ExperimentId")] Tests test)
        {
            if (id != test.TestId)
            {
                return NotFound();
            }

            if (!DoesUserHaveAccess(test.ExperimentId))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync().ConfigureAwait(true);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestsExists(test.TestId))
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

            return View(test);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testVm = await _context.Tests.
                Select(m =>
                new TestDeleteViewModel
                {
                    TestId = m.TestId,
                    Name = m.Name,
                    Metadata = m.Metadata,
                    StartedTime = m.StartedTime,
                    EndedTime = m.EndedTime,
                    ExperimentId = m.ExperimentId
                }).
                FirstOrDefaultAsync(m => m.TestId == id).ConfigureAwait(true);

            if (testVm == null)
            {
                return NotFound();
            }

            if (!DoesUserHaveAccess(testVm.ExperimentId))
            {
                return NotFound();
            }

            testVm.ExperimentName = _context.Experiments.FirstOrDefault(m => m.ExperimentId == testVm.ExperimentId).Name;
            return View(testVm);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var test = await _context.Tests.FindAsync(id).ConfigureAwait(true);
            var experimentId = test.ExperimentId;

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync().ConfigureAwait(true);

            return RedirectToAction("Experiment", new { id = experimentId });
        }

        private bool TestsExists(int id)
        {
            return _context.Tests.Any(t => t.TestId == id);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Укажите IFormatProvider", Justification = "<Ожидание>")]
        private int GetCurrUserId()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value);
        }

        private bool DoesUserHaveAccess(int experimentId)
        {
            return _context.UserExperiments.Any(ue => ue.UserId == GetCurrUserId() && ue.ExperimentId == experimentId);
        }
    }
}
