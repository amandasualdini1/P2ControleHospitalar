using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P2Hospital.Data;
using P2Hospital.Models;

namespace P2Hospital.Controllers
{
    [Authorize]
    public class OPMEController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OPMEController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OPMEs
        public async Task<IActionResult> Index()
        {
            return View(await _context.OPME.Include(e => e.Enfermeiro).ToListAsync());
        }

        // GET: OPMEs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oPME = await _context.OPME.Include(e => e.Enfermeiro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oPME == null)
            {
                return NotFound();
            }

            return View(oPME);
        }

        // GET: OPMEs/Create
        public IActionResult Create()
        {
            var opme = new OPME();
            var enfermeiro = _context.Enfermeiro.ToList();

            opme.Enfermeiros = new List<SelectListItem>();

            foreach (var enf in enfermeiro)
            {
                opme.Enfermeiros.Add(new SelectListItem { Text = enf.Nome, Value = enf.Id.ToString() });
            }
            return View(opme);
        }

        // POST: OPMEs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,TipoClassificao")] OPME oPME)
        {
            int _enfermeiroId = int.Parse(Request.Form["Enfermeiro"].ToString());
            var enfermeiro = _context.Enfermeiro.FirstOrDefault(e => e.Id == _enfermeiroId);
            oPME.Enfermeiro = enfermeiro;

            if (ModelState.IsValid)
            {
                _context.Add(oPME);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oPME);
        }

        // GET: OPMEs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var opme = _context.MaterialConsumo.Include(e => e.Enfermeiro).First(mc => mc.Id == id);

            var enfermeiro = _context.Enfermeiro.ToList();

            opme.Enfermeiros = new List<SelectListItem>();

            foreach (var enf in enfermeiro)
            {
                opme.Enfermeiros.Add(new SelectListItem { Text = enf.Nome, Value = enf.Id.ToString() });
            }

            if (opme == null)
            {
                return NotFound();
            }
            return View(opme);
        }

        // POST: OPMEs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,TipoClassificao")] OPME oPME)
        {
            if (id != oPME.Id)
            {
                return NotFound();
            }

            int _enfermeiroId = int.Parse(Request.Form["Enfermeiro"].ToString());
            var enfermeiro = _context.Enfermeiro.FirstOrDefault(e => e.Id == _enfermeiroId);
            oPME.Enfermeiro = enfermeiro;
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oPME);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OPMEExists(oPME.Id))
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
            return View(oPME);
        }

        // GET: OPMEs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oPME = await _context.OPME.Include(e => e.Enfermeiro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (oPME == null)
            {
                return NotFound();
            }

            return View(oPME);
        }

        // POST: OPMEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oPME = await _context.OPME.FindAsync(id);
            _context.OPME.Remove(oPME);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OPMEExists(int id)
        {
            return _context.OPME.Any(e => e.Id == id);
        }
    }
}
