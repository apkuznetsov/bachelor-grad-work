using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webapp.Models;
using WebappDb;

namespace Webapp.Controllers
{
    public class SensorsController : Controller
    {
        private readonly webappdbContext _context;

        public SensorsController(webappdbContext context)
        {
            _context = context;
        }

        // GET: Sensors
        public async Task<IActionResult> Index()
        {
            var webappdbContext = _context.Sensors.Include(s => s.CommunicationProtocol).Include(s => s.DataType);
            return View(await webappdbContext.ToListAsync());
        }

        // GET: Sensors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensors = await _context.Sensors
                .Include(s => s.CommunicationProtocol)
                .Include(s => s.DataType)
                .FirstOrDefaultAsync(m => m.SensorId == id);
            if (sensors == null)
            {
                return NotFound();
            }

            return View(sensors);
        }

        // GET: Sensors/Create
        public IActionResult Create()
        {
            ViewData["CommunicationProtocolId"] = new SelectList(
                _context.CommunicationProtocols,
                "CommunicationProtocolId",
                "ProtocolName");

            return View();
        }

        // POST: Sensors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SensorId,Name,Metadata,DataType,CommunicationProtocolId,IpAddress,Port")] SensorViewModel sensorVm)
        {
            if (ModelState.IsValid)
            {
                Datatypes newDatatype = new Datatypes();
                newDatatype.Schema = sensorVm.DataType;
                _context.Add(newDatatype);
                await _context.SaveChangesAsync();

                Sensors newSensor = new Sensors();
                newSensor.SensorId = sensorVm.SensorId;
                newSensor.Name = sensorVm.Name;
                newSensor.Metadata = sensorVm.Metadata;
                newSensor.CommunicationProtocolId = sensorVm.CommunicationProtocolId;
                newSensor.IpAddress = sensorVm.IpAddress;
                newSensor.Port = sensorVm.Port;
                newSensor.DataTypeId = newDatatype.DataTypeId;

                _context.Add(newSensor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CommunicationProtocolId"] = new SelectList(
                _context.CommunicationProtocols,
                "CommunicationProtocolId",
                "ProtocolName",
                sensorVm.CommunicationProtocolId);

            return View(sensorVm);
        }

        // GET: Sensors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensors = await _context.Sensors.FindAsync(id);
            if (sensors == null)
            {
                return NotFound();
            }
            ViewData["CommunicationProtocolId"] = new SelectList(_context.CommunicationProtocols, "CommunicationProtocolId", "ProtocolName", sensors.CommunicationProtocolId);
            ViewData["DataTypeId"] = new SelectList(_context.Datatypes, "DataTypeId", "Metadata", sensors.DataTypeId);
            return View(sensors);
        }

        // POST: Sensors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SensorId,Metadata,DataTypeId,IpAddress,Port,CommunicationProtocolId")] Sensors sensors)
        {
            if (id != sensors.SensorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sensors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SensorsExists(sensors.SensorId))
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
            ViewData["CommunicationProtocolId"] = new SelectList(_context.CommunicationProtocols, "CommunicationProtocolId", "ProtocolName", sensors.CommunicationProtocolId);
            ViewData["DataTypeId"] = new SelectList(_context.Datatypes, "DataTypeId", "Metadata", sensors.DataTypeId);
            return View(sensors);
        }

        // GET: Sensors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sensors = await _context.Sensors
                .Include(s => s.CommunicationProtocol)
                .Include(s => s.DataType)
                .FirstOrDefaultAsync(m => m.SensorId == id);
            if (sensors == null)
            {
                return NotFound();
            }

            return View(sensors);
        }

        // POST: Sensors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sensors = await _context.Sensors.FindAsync(id);
            _context.Sensors.Remove(sensors);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SensorsExists(int id)
        {
            return _context.Sensors.Any(e => e.SensorId == id);
        }
    }
}
