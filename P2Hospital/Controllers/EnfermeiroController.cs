using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P2Hospital.Data;
using P2Hospital.Models;

namespace P2Hospital.Controllers
{
    [Authorize]
    public class EnfermeiroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EnfermeiroController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Enfermeiroes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Enfermeiro.ToListAsync());
        }

        // GET: Enfermeiroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enfermeiro = await _context.Enfermeiro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enfermeiro == null)
            {
                return NotFound();
            }

            return View(enfermeiro);
        }

        // GET: Enfermeiroes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Enfermeiroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CPF,CodigoInternoEnfermeiro,Description,ImageFile")] Enfermeiro enfermeiro)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(enfermeiro.ImageFile.FileName);
                string extension = Path.GetExtension(enfermeiro.ImageFile.FileName);
                enfermeiro.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image", fileName);
              
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await enfermeiro.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(enfermeiro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(enfermeiro);
        }

        // GET: Enfermeiroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enfermeiro = await _context.Enfermeiro.FindAsync(id);
            if (enfermeiro == null)
            {
                return NotFound();
            }
            return View(enfermeiro);
        }

        // POST: Enfermeiroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPF,CodigoInternoEnfermeiro,Description,Image")] Enfermeiro enfermeiro)
        {
            if (id != enfermeiro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var productCompare = _context.Enfermeiro.Find(enfermeiro.Id);

                    enfermeiro.Image = (enfermeiro.ImageFile == null) ? "" : enfermeiro.ImageFile.FileName;

                    if (!CompareFileName(productCompare.Image, enfermeiro.Image))
                    {
                        //Remover Imagem anterior
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", productCompare.Image);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        //Incluir nova
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(enfermeiro.ImageFile.FileName);
                        string extension = Path.GetExtension(enfermeiro.ImageFile.FileName);
                        enfermeiro.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/image", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await enfermeiro.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    productCompare.Description = enfermeiro.Description;
                    productCompare.Image = string.IsNullOrEmpty(enfermeiro.Image) ? productCompare.Image : enfermeiro.Image;

                    _context.Update(enfermeiro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnfermeiroExists(enfermeiro.Id))
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
            return View(enfermeiro);
        }


        private bool CompareFileName(string name, string newName)
        {
            //Se não foi selecionada uma imagem nova fica a antiga. 
            if (string.IsNullOrEmpty(newName))
                return true;

            if (string.IsNullOrEmpty(name))
                return false;

            //extensão do arquivo
            var validateName = name.Split('.');
            var validateNewName = newName.Split('.');

            if (validateName[1] != validateNewName[1])
                return false;

            //Remover a data gerada
            string nameOld = validateName[0].Replace(validateName[0]
                                            .Substring(validateName[0].Length - 9, 9), "");

            if (newName == nameOld)
                return true;

            return false;
        }


        // GET: Enfermeiroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enfermeiro = await _context.Enfermeiro
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enfermeiro == null)
            {
                return NotFound();
            }

            return View(enfermeiro);
        }

        // POST: Enfermeiroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Enfermeiro.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", product.Image);

            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            var enfermeiro = await _context.Enfermeiro.FindAsync(id);
            _context.Enfermeiro.Remove(enfermeiro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnfermeiroExists(int id)
        {
            return _context.Enfermeiro.Any(e => e.Id == id);
        }
    }
}
