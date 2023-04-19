using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion.models;
using aplicacion.ViewModels;


namespace aplicacion.Controllers
{
    public class MultipropietariosController : Controller
    {
        private readonly EscriturasContext _context;

        public MultipropietariosController(EscriturasContext context)
        {
            _context = context;
        }

        // GET: Multipropietarios
        public async Task<IActionResult> Index(string searchString, int searchDate, int searchManzana, int searchPredio)
        {
            if (_context.Multipropietarios is null)
            {
                return Problem("Entity set 'EscriturasContext.Multipropietarios' is null.");
            }

            var Multi = from m in _context.Multipropietarios
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                Multi = Multi.Where(s => s.Comuna!.Contains(searchString));
            }

            if (searchManzana != 0)
            {
                Multi = Multi.Where(s => s.Manzana == searchManzana);
            }

            if (searchPredio != 0)
            {
                Multi = Multi.Where(s => s.Predio == searchPredio);
            }

            if (searchDate != 0 && searchDate >= 2019)
            {
                Multi = Multi.Where(s => s.AnoVigenciaInicial <= searchDate && (s.AnoVigenciaFinal >= searchDate || s.AnoVigenciaFinal == 0));
            }

            var nombresComunas = new Models.ConstantsAndList();
            var viewModel = new MultipropietarioViewModel
            {
                Multipropietarios = await Multi.ToListAsync(),
                Comunas = nombresComunas.ListComunas
            };

            return View(viewModel);
        }


        // GET: Multipropietarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Multipropietarios == null)
            {
                return NotFound();
            }

            var multipropietario = await _context.Multipropietarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multipropietario == null)
            {
                return NotFound();
            }

            return View(multipropietario);
        }

        // GET: Multipropietarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Multipropietarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comuna,Manzana,Predio,RunRut,PorcentajeDerecho,Fojas,AnoInscripcion,NumeroInscripcion,FechaInscripcion,AnoVigenciaInicial,AnoVigenciaFinal")] Multipropietario multipropietario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(multipropietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(multipropietario);
        }

        // GET: Multipropietarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Multipropietarios == null)
            {
                return NotFound();
            }

            var multipropietario = await _context.Multipropietarios.FindAsync(id);
            if (multipropietario == null)
            {
                return NotFound();
            }
            return View(multipropietario);
        }

        // POST: Multipropietarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comuna,Manzana,Predio,RunRut,PorcentajeDerecho,Fojas,AnoInscripcion,NumeroInscripcion,FechaInscripcion,AnoVigenciaInicial,AnoVigenciaFinal")] Multipropietario multipropietario)
        {
            if (id != multipropietario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multipropietario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultipropietarioExists(multipropietario.Id))
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
            return View(multipropietario);
        }

        // GET: Multipropietarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Multipropietarios == null)
            {
                return NotFound();
            }

            var multipropietario = await _context.Multipropietarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (multipropietario == null)
            {
                return NotFound();
            }

            return View(multipropietario);
        }

        // POST: Multipropietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Multipropietarios == null)
            {
                return Problem("Entity set 'EscriturasContext.Multipropietarios'  is null.");
            }
            var multipropietario = await _context.Multipropietarios.FindAsync(id);
            if (multipropietario != null)
            {
                _context.Multipropietarios.Remove(multipropietario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MultipropietarioExists(int id)
        {
          return (_context.Multipropietarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
