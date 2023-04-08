using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion.models;
using aplicacion.ViewModels;
using Azure.Core;
using System.Globalization;
using Newtonsoft.Json;

namespace aplicacion.Controllers
{
    public class EscriturasController : Controller
    {
        private EscriturasContext db = new EscriturasContext();
        private readonly EscriturasContext _context;

        public EscriturasController(EscriturasContext context)
        {
            _context = context;
        }

        // GET: Escrituras
        public async Task<IActionResult> Index()
        {
              return _context.Escrituras != null ? 
                          View(await _context.Escrituras.ToListAsync()) :
                          Problem("Entity set 'EscriturasContext.Escrituras'  is null.");
        }

        // GET: Escrituras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Escrituras == null)
            {
                return NotFound();
            }

            var escritura = await _context.Escrituras
                .FirstOrDefaultAsync(m => m.NumAtencion == id);
            if (escritura == null)
            {
                return NotFound();
            }
            var enajenantesActuales = db.Enajenantes
                    .Where(b => b.NumAtencion == id)
                    .ToList();
            var adquirientesActuales = db.Adquirentes
                    .Where(c => c.NumAtencion == id)
                    .ToList();
            System.Diagnostics.Debug.WriteLine(adquirientesActuales);
            foreach (var enajenanteActual in adquirientesActuales)
            {
                System.Diagnostics.Debug.WriteLine(enajenanteActual.ToString());
            }
            ViewBag.EnajenantesActuales = enajenantesActuales;
            ViewBag.AdquirientesActuales = adquirientesActuales;
            return View(escritura);
        }

        // GET: Escrituras/Create
        public IActionResult Create()
        {
            var dbContext = new EscriturasContext();
            var ultimoNumAtencion = dbContext.Escrituras.OrderByDescending(e => e.FechaInscripcion).Select(e => e.NumAtencion).FirstOrDefault();
            ultimoNumAtencion += 1;
            var enan = new Enajenante();
            ViewBag.NumAtencion = ultimoNumAtencion;
            var model = new EscrituraViewModel();
           

            // Llamada a la API para obtener todas las comunas de Chile
            var client = new HttpClient();
            var comunas = new List<Comuna>();
            /*
            for (int i = 1; i <= 16; i++)
            {
                var response = client.GetAsync($"https://apis.digital.gob.cl/dpa/regiones/{i}/comunas").Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var regionComunas = JsonConvert.DeserializeObject<List<Comuna>>(result);
                comunas.AddRange(regionComunas);
            }
            */  
            model.Comunas = comunas;

            return View(model);

        }

        // POST: Escrituras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EscrituraViewModel escrituraViewModel)
        {
            Console.WriteLine("Hola como estas");
            var dbContext = new EscriturasContext();
            var NumAtencion = dbContext.Escrituras.OrderByDescending(e => e.FechaInscripcion).Select(e => e.NumAtencion).FirstOrDefault();
            NumAtencion = NumAtencion + 1;
            Console.WriteLine("entre a if ");
            Console.WriteLine(escrituraViewModel.Escritura.Cne);
            var escritura = new Escritura
            {
                Cne = escrituraViewModel.Escritura.Cne,
                Comuna = escrituraViewModel.Escritura.Comuna,
                Manzana = escrituraViewModel.Escritura.Manzana,
                Predio = escrituraViewModel.Escritura.Predio,
                Fojas = escrituraViewModel.Escritura.Fojas,
                FechaInscripcion = escrituraViewModel.Escritura.FechaInscripcion,
                NumeroInscripcion = escrituraViewModel.Escritura.NumeroInscripcion
            };
            Console.WriteLine("Cree escritura");
           
            var EnajenateRun = (Request.Form["Enajenate.RunRut"].ToString()).Split(",");
            var EnajenantePorcentajeDerecho = (Request.Form["Enajenante.PorcentajeDerecho"].ToString()).Split(",");
            //List<double> EnajenantePorcentajeDerecho = EPorcentajeDerecho.ConvertAll(x => double.Parse(x));
            var EnajenanteNumAtencion = (Request.Form["Enajenante.NumAtencion"].ToString()).Split(",");
            var EnajenantePorcentajeDerechoNoAcreditado = (Request.Form["Enajenante.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
            for (int i = 0; i < EnajenateRun.Length; i++)
            {
                var adquirente = new Adquirente
                {
                    NumAtencion = int.Parse(EnajenanteNumAtencion[i]),
                    RunRut = EnajenateRun[i],
                    PorcentajeDerecho = double.Parse(EnajenantePorcentajeDerecho[i]),
                    PorcentajeDerechoNoAcreditado = bool.Parse(EnajenantePorcentajeDerechoNoAcreditado[i]),
                    NumAtencionNavigation = escritura
                };

                escritura.Adquirentes.Add(adquirente);

            }

            if (escritura.Cne == "compraventa")
            {
                var AdquirienteRun = (Request.Form["Adquirente.RunRut"].ToString()).Split(",");
                var AdquirentePorcentajeDerecho = (Request.Form["Adquirente.PorcentajeDerecho"].ToString()).Split(",");
                //List<double> EnajenantePorcentajeDerecho = EPorcentajeDerecho.ConvertAll(x => double.Parse(x));
                var AdquirenteNumAtencion = (Request.Form["Adquirente.NumAtencion"].ToString()).Split(",");
                var AdquirentePorcentajeDerechoNoAcreditado = (Request.Form["Adquirente.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");

                for (int i = 0; i < AdquirienteRun.Length; i++)
                {
                    var enajenante = new Enajenante
                    {
                        NumAtencion = int.Parse(EnajenanteNumAtencion[i]),
                        RunRut = AdquirienteRun[i],
                        PorcentajeDerecho = double.Parse(AdquirentePorcentajeDerecho[i]),
                        PorcentajeDerechoNoAcreditado = bool.Parse(AdquirentePorcentajeDerechoNoAcreditado[i]),
                        NumAtencionNavigation = escritura
                    };

                    escritura.Enajenantes.Add(enajenante);

                }
            }

            _context.Escrituras.Add(escritura);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
            
            //return View(escrituraViewModel);
        }
        // POST: Escrituras/CreateMultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateMultiple(List<Adquirente> adquirentes, Escritura escritura)
        {
            if (ModelState.IsValid)
            {
                foreach (var adquirente in adquirentes)
                {
                    adquirente.NumAtencionNavigation = escritura;
                    _context.Add(adquirente);
                }

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(escritura);
        }

        // GET: Escrituras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Escrituras == null)
            {
                return NotFound();
            }

            var escritura = await _context.Escrituras.FindAsync(id);
            if (escritura == null)
            {
                return NotFound();
            }
            return View(escritura);
        }

        // POST: Escrituras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NumAtencion,Cne,Comuna,Manzana,Predio,Fojas,FechaInscripcion,NumeroInscripcion")] Escritura escritura)
        {
            if (id != escritura.NumAtencion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(escritura);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscrituraExists(escritura.NumAtencion))
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
            return View(escritura);
        }

      
        // GET: Escrituras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Escrituras == null)
            {
                return NotFound();
            }

            var escritura = await _context.Escrituras
                .FirstOrDefaultAsync(m => m.NumAtencion == id);
            if (escritura == null)
            {
                return NotFound();
            }

            return View(escritura);
        }

        // POST: Escrituras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Escrituras == null)
            {
                return Problem("Entity set 'EscriturasContext.Escrituras'  is null.");
            }
            var escritura = await _context.Escrituras.FindAsync(id);
            if (escritura != null)
            {
                _context.Escrituras.Remove(escritura);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscrituraExists(int id)
        {
          return (_context.Escrituras?.Any(e => e.NumAtencion == id)).GetValueOrDefault();
        }
    }
}
