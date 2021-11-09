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
    public class MaterialConsumoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialConsumoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MaterialConsumoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaterialConsumo.Include(e => e.Enfermeiro).ToListAsync());
        }

        // GET: MaterialConsumoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialConsumo = await _context.MaterialConsumo.Include(e => e.Enfermeiro)
                      .FirstOrDefaultAsync(m => m.Id == id);
            if (materialConsumo == null)
            {
                return NotFound();
            }

            return View(materialConsumo);
        }

        // GET: MaterialConsumoes/Create
        public IActionResult Create()
        {
            var mc = new MaterialConsumo();
            var enfermeiro = _context.Enfermeiro.ToList();

            mc.Enfermeiros = new List<SelectListItem>();

            foreach (var enf in enfermeiro)
            {
                mc.Enfermeiros.Add(new SelectListItem { Text = enf.Nome, Value = enf.Id.ToString() });
            }
            return View(mc);
        }

        // POST: MaterialConsumoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,QtdProduto")] MaterialConsumo materialConsumo)
        {
            int _enfermeiroId = int.Parse(Request.Form["Enfermeiro"].ToString());
            var enfermeiro = _context.Enfermeiro.FirstOrDefault(e => e.Id == _enfermeiroId);
            materialConsumo.Enfermeiro = enfermeiro;

            if (ModelState.IsValid)
            {
                _context.Add(materialConsumo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materialConsumo);
        }

        // GET: MaterialConsumoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialConsumo = _context.MaterialConsumo.Include(e => e.Enfermeiro).First(mc => mc.Id == id);

            var enfermeiro = _context.Enfermeiro.ToList();

            materialConsumo.Enfermeiros = new List<SelectListItem>();

            foreach (var enf in enfermeiro)
            {
                materialConsumo.Enfermeiros.Add(new SelectListItem { Text = enf.Nome, Value = enf.Id.ToString() });
            }

            if (materialConsumo == null)
            {
                return NotFound();
            }
            return View(materialConsumo);
        }

        // POST: MaterialConsumoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,QtdProduto")] MaterialConsumo materialConsumo)
        {
            if (id != materialConsumo.Id)
            {
                return NotFound();
            }

            int _enfermeiroId = int.Parse(Request.Form["Enfermeiro"].ToString());
            var enfermeiro = _context.Enfermeiro.FirstOrDefault(e => e.Id == _enfermeiroId);
            materialConsumo.Enfermeiro = enfermeiro;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialConsumo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialConsumoExists(materialConsumo.Id))
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
            return View(materialConsumo);
        }

        // GET: MaterialConsumoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialConsumo = await _context.MaterialConsumo.Include(e => e.Enfermeiro)
        .FirstOrDefaultAsync(m => m.Id == id);
            if (materialConsumo == null)
            {
                return NotFound();
            }

            return View(materialConsumo);
        }

        // POST: MaterialConsumoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialConsumo = await _context.MaterialConsumo.FindAsync(id);
            _context.MaterialConsumo.Remove(materialConsumo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialConsumoExists(int id)
        {
            return _context.MaterialConsumo.Any(e => e.Id == id);
        }
    }
}
