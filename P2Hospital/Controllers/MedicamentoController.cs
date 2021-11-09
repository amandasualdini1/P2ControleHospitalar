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
    public class MedicamentoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicamentoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicamentoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicamento.Include(e => e.Enfermeiro).ToListAsync());
        }

        // GET: Medicamentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Medicamento.Include(e => e.Enfermeiro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicamento == null)
            {
                return NotFound();
            }

            return View(medicamento);
        }

        // GET: Medicamentoes/Create
        public IActionResult Create()
        {
            var medicamento = new Medicamento();
            var enfermeiro = _context.Enfermeiro.ToList();

            medicamento.Enfermeiros = new List<SelectListItem>();

            foreach (var enf in enfermeiro)
            {
                medicamento.Enfermeiros.Add(new SelectListItem { Text = enf.Nome, Value = enf.Id.ToString() });
            }
            return View(medicamento);
        }

        // POST: Medicamentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Dosagem,UnidadeDosagem")] Medicamento medicamento)
        {

            int _enfermeiroId = int.Parse(Request.Form["Enfermeiro"].ToString());
            var enfermeiro = _context.Enfermeiro.FirstOrDefault(e => e.Id == _enfermeiroId);
            medicamento.Enfermeiro = enfermeiro;

            if (ModelState.IsValid)
            {
                _context.Add(medicamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicamento);
        }

        // GET: Medicamentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var medicamento = _context.Medicamento.Include(e => e.Enfermeiro).First(mc => mc.Id == id);

            var enfermeiro = _context.Enfermeiro.ToList();

            medicamento.Enfermeiros = new List<SelectListItem>();

            foreach (var enf in enfermeiro)
            {
                medicamento.Enfermeiros.Add(new SelectListItem { Text = enf.Nome, Value = enf.Id.ToString() });
            }

            if (id == null)
            {
                return NotFound();
            }

            if (medicamento == null)
            {
                return NotFound();
            }
            return View(medicamento);
        }

        // POST: Medicamentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Dosagem,UnidadeDosagem")] Medicamento medicamento)
        {

            if (id != medicamento.Id)
            {
                return NotFound();
            }

            int _enfermeiroId = int.Parse(Request.Form["Enfermeiro"].ToString());
            var enfermeiro = _context.Enfermeiro.FirstOrDefault(e => e.Id == _enfermeiroId);
            medicamento.Enfermeiro = enfermeiro;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicamentoExists(medicamento.Id))
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
            return View(medicamento);
        }

        // GET: Medicamentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicamento = await _context.Medicamento
                .Include(e => e.Enfermeiro)
                .FirstOrDefaultAsync(m => m.Id == id); ;
            if (medicamento == null)
            {
                return NotFound();
            }

            return View(medicamento);
        }

        // POST: Medicamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicamento = await _context.Medicamento.FindAsync(id);
            _context.Medicamento.Remove(medicamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamento.Any(e => e.Id == id);
        }
    }
}
