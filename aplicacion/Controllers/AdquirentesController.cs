using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion.models;

namespace aplicacion.Controllers
{
    public class AdquirentesController : Controller
    {
        private readonly EscriturasContext _context;

        public AdquirentesController(EscriturasContext context)
        {
            _context = context;
        }

        // GET: Adquirentes
        public async Task<IActionResult> Index()
        {
            var escriturasContext = _context.Adquirentes.Include(a => a.NumAtencionNavigation);
            return View(await escriturasContext.ToListAsync());
        }

        // GET: Adquirentes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Adquirentes == null)
            {
                return NotFound();
            }

            var adquirente = await _context.Adquirentes
                .Include(a => a.NumAtencionNavigation)
                .FirstOrDefaultAsync(m => m.IdAdquirente == id);
            if (adquirente == null)
            {
                return NotFound();
            }

            return View(adquirente);
        }

        // GET: Adquirentes/Create
        public IActionResult Create()
        {
            ViewData["NumAtencion"] = new SelectList(_context.Escrituras, "NumAtencion", "NumAtencion");
            return View();
        }

        // POST: Adquirentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAdquirente,NumAtencion,RunRut,PorcentajeDerecho,PorcentajeDerechoNoAcreditado")] Adquirente adquirente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adquirente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumAtencion"] = new SelectList(_context.Escrituras, "NumAtencion", "NumAtencion", adquirente.NumAtencion);
            return View(adquirente);
        }

        // GET: Adquirentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Adquirentes == null)
            {
                return NotFound();
            }

            var adquirente = await _context.Adquirentes.FindAsync(id);
            if (adquirente == null)
            {
                return NotFound();
            }
            ViewData["NumAtencion"] = new SelectList(_context.Escrituras, "NumAtencion", "NumAtencion", adquirente.NumAtencion);
            return View(adquirente);
        }

        // POST: Adquirentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAdquirente,NumAtencion,RunRut,PorcentajeDerecho,PorcentajeDerechoNoAcreditado")] Adquirente adquirente)
        {
            if (id != adquirente.IdAdquirente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adquirente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdquirenteExists(adquirente.IdAdquirente))
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
            ViewData["NumAtencion"] = new SelectList(_context.Escrituras, "NumAtencion", "NumAtencion", adquirente.NumAtencion);
            return View(adquirente);
        }

        // GET: Adquirentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Adquirentes == null)
            {
                return NotFound();
            }

            var adquirente = await _context.Adquirentes
                .Include(a => a.NumAtencionNavigation)
                .FirstOrDefaultAsync(m => m.IdAdquirente == id);
            if (adquirente == null)
            {
                return NotFound();
            }

            return View(adquirente);
        }

        // POST: Adquirentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Adquirentes == null)
            {
                return Problem("Entity set 'EscriturasContext.Adquirentes'  is null.");
            }
            var adquirente = await _context.Adquirentes.FindAsync(id);
            if (adquirente != null)
            {
                _context.Adquirentes.Remove(adquirente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdquirenteExists(int id)
        {
          return (_context.Adquirentes?.Any(e => e.IdAdquirente == id)).GetValueOrDefault();
        }
    }
}
