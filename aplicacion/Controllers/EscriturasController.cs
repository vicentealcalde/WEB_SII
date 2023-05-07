﻿using System;
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
using System.Web;
using Microsoft.AspNetCore.Http;


namespace aplicacion.Controllers
{
    public class EscriturasController : Controller
    {
        private EscriturasContext db = new EscriturasContext();
        private readonly EscriturasContext _context;

        public double DIfSumPercent(List<string> porcentaje){
            double sumaPorcentajeDerecho = 0.0;
            double restaPorcentaje = 0.0;

            for (int i = 0; i < porcentaje.Count; i++)
            {
                sumaPorcentajeDerecho += double.Parse(porcentaje[i]);
            }
            restaPorcentaje = 100 - sumaPorcentajeDerecho;
            return restaPorcentaje;
        }

        public List<Multipropietario> ObtenerMultipropietarios(List<string> ruts)
        {
            List<Multipropietario> multipropietarios = new List<Multipropietario>();
            
            foreach (string rut in ruts)
            {
                Multipropietario multipropietario = _context.Multipropietarios.Where(mp => mp.RunRut == rut).OrderByDescending(mp => mp.AnoVigenciaInicial).FirstOrDefault();
                if (multipropietario != null)
                {
                    multipropietarios.Add(multipropietario);
                }
            }
            
            return multipropietarios;
        }
        public List<string> ProcesarPorcentajes(List<string> porcentajes)
        {
            // Convertir los porcentajes a números decimales
            List<decimal> porcentajesDecimales = porcentajes.Select(p => decimal.Parse(p)).ToList();
            
            // Calcular la suma de los porcentajes
            decimal sumaPorcentajes = porcentajesDecimales.Sum();
            
            // Calcular la cantidad de porcentajes que son cero
            int cantidadCeros = porcentajesDecimales.Count(p => p == 0);
            
            // Calcular la diferencia entre la suma de los porcentajes y 100
            decimal diferencia = 100 - sumaPorcentajes;
            
            // Si la diferencia es mayor a cero y hay porcentajes iguales a cero, reemplazar los ceros
            if (diferencia > 0 && cantidadCeros > 0)
            {
                // Calcular la cantidad a distribuir entre los porcentajes iguales a cero
                decimal cantidadDistribuir = diferencia / cantidadCeros;
                
                // Reemplazar los ceros con la cantidad a distribuir
                for (int i = 0; i < porcentajesDecimales.Count; i++)
                {
                    if (porcentajesDecimales[i] == 0)
                    {
                        porcentajesDecimales[i] = cantidadDistribuir;
                    }
                }
            }
            
            // Convertir los porcentajes de vuelta a string y retornarlos en una lista
            List<string> porcentajesActualizados = porcentajesDecimales.Select(p => p.ToString()).ToList();
            return porcentajesActualizados;
        }

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


        public bool ValidarMultipropietarios(List<string> ruts)
        {
            foreach (string rut in ruts)
            {
                if (!_context.Multipropietarios.Any(mp => mp.RunRut == rut))
                {
                    return false;
                }
            }
            return true;
        }
        // GET: Escrituras/Create
        public IActionResult Create()
        {
            var dbContext = new EscriturasContext();
            var ultimoNumAtencion = dbContext.Escrituras.OrderByDescending(e => e.FechaInscripcion).Select(e => e.NumAtencion).FirstOrDefault();
            ultimoNumAtencion += 1;
            var enan = new Enajenante();
            var adqu = new Adquirente();
            ViewBag.NumAtencion = ultimoNumAtencion;
            var model = new EscrituraViewModel();
            var nombresComunas = new Models.ConstantsAndList();
            model.Comunas = nombresComunas.ListComunas;
            return View(model);

        }

        // POST: Escrituras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EscrituraViewModel escrituraViewModel)
        {

            var dbContext = new EscriturasContext();
            var NumAtencion = dbContext.Escrituras.OrderByDescending(e => e.FechaInscripcion).Select(e => e.NumAtencion).FirstOrDefault();
            NumAtencion = NumAtencion + 1;


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

            if (escritura.Cne == "compraventa")
            {
                var EnajenanteNumAtencion = (Request.Form["Enajenante.NumAtencion"].ToString()).Split(",");
                var AdquirienteNumAtencion = (Request.Form["Adquiriente.NumAtencion"].ToString()).Split(",");
                var EnajenateRun = (Request.Form["Enajenate.RunRut"].ToString()).Split(",");
                var EnajenantePorcentajeDerecho = (Request.Form["Enajenante.PorcentajeDerecho"].ToString()).Split(",");
                var EnajenantePorcentajeDerechoNoAcreditado = (Request.Form["Enajenante.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
                var AdquirienteRun = (Request.Form["Adquirente.RunRut"].ToString()).Split(",");
                var AdquirentePorcentajeDerecho = (Request.Form["Adquirente.PorcentajeDerecho"].ToString()).Split(",");
                var AdquirentePorcentajeDerechoNoAcreditado = (Request.Form["Adquirente.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
                var IsValidEnajenantes = ValidarMultipropietarios(EnajenateRun.ToList());

                if (IsValidEnajenantes == false){
                    return Create();
                }
                AdquirentePorcentajeDerecho = ProcesarPorcentajes(AdquirentePorcentajeDerecho.ToList()).ToArray();
                var DifSumPercentAdquiriente = DIfSumPercent(AdquirentePorcentajeDerecho.ToList());
                var DifSumPercentEnajenate = DIfSumPercent(EnajenantePorcentajeDerecho.ToList());
                
                if (DifSumPercentAdquiriente == 0)
                {


                }



            }

            if (escritura.Cne == "regularizacion")
            {
                var AdquirienteRun = (Request.Form["Adquirente.RunRut"].ToString()).Split(",");
                var AdquirentePorcentajeDerecho = (Request.Form["Adquirente.PorcentajeDerecho"].ToString()).Split(",");
                var AdquirentePorcentajeDerechoNoAcreditado = (Request.Form["Adquirente.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
                double porcentajeConcedido = 0;
                int cantidad = 0;

                for (int i = 0; i < AdquirienteRun.Length; i++)
                {
                    var updateList = AdquirentePorcentajeDerecho.ToList();

                    if (bool.Parse(AdquirentePorcentajeDerechoNoAcreditado[i]) && i < AdquirentePorcentajeDerechoNoAcreditado.Length)
                    {
                        updateList.Add("0");
                        cantidad++;
                    }

                    AdquirentePorcentajeDerecho = updateList.ToArray();

                    try
                    {
                        porcentajeConcedido += double.Parse(AdquirentePorcentajeDerecho[i]);
                    }
                    catch
                    {
                        updateList.Add("0");
                        cantidad++;
                        AdquirentePorcentajeDerecho = updateList.ToArray();
                        porcentajeConcedido += double.Parse(AdquirentePorcentajeDerecho[i]);
                    }
                }

                for (int i = 0; i < AdquirienteRun.Length; i++)
                {
                    var enajenante = new Enajenante
                    {
                        NumAtencion = int.Parse(escrituraViewModel.Escritura.NumeroInscripcion),
                        RunRut = AdquirienteRun[i],
                        PorcentajeDerecho = double.Parse(AdquirentePorcentajeDerecho[i]),
                        PorcentajeDerechoNoAcreditado = bool.Parse(AdquirentePorcentajeDerechoNoAcreditado[i]),
                        NumAtencionNavigation = escritura
                    };

                    escritura.Enajenantes.Add(enajenante);
                }

                var multipropietarios = _context.Multipropietarios
                    .Where(m => m.Comuna == escrituraViewModel.Escritura.Comuna &&
                                m.Manzana == int.Parse(escrituraViewModel.Escritura.Manzana) &&
                                m.Predio == int.Parse(escrituraViewModel.Escritura.Predio))
                    .ToList();

                foreach (var multipropietario in multipropietarios)
                {
                    if (multipropietario.AnoVigenciaFinal == 0)
                    {
                        multipropietario.AnoVigenciaFinal = escrituraViewModel.Escritura.FechaInscripcion.Year;
                        _context.Update(multipropietario);
                    }
                }


                double sumaPorcentajeDerecho = 0.0;
                double restaPorcentaje = 0.0;

                for (int i = 0; i < AdquirienteRun.Length; i++)
                {
                    sumaPorcentajeDerecho += double.Parse(AdquirentePorcentajeDerecho[i]);
                }
                restaPorcentaje = 100 - sumaPorcentajeDerecho;
                double promedioPorcentajeDerecho = restaPorcentaje / AdquirienteRun.Length;

                for (int i = 0; i < AdquirienteRun.Length; i++)
                {
                    DateTime preYear = escrituraViewModel.Escritura.FechaInscripcion;

                    if (escrituraViewModel.Escritura.FechaInscripcion.Year < 2019)
                    {
                        preYear = new DateTime(2019, escrituraViewModel.Escritura.FechaInscripcion.Month, escrituraViewModel.Escritura.FechaInscripcion.Day);
                    }

                    var multipropietarioEqualYear = multipropietarios
                        .Where(a => a.Comuna == escrituraViewModel.Escritura.Comuna)
                        .Where(b => b.Manzana == int.Parse(escrituraViewModel.Escritura.Manzana))
                        .Where(c => c.Predio == int.Parse(escrituraViewModel.Escritura.Predio))
                        .Where(d => d.AnoVigenciaInicial == int.Parse(preYear.Year.ToString()))
                        .Where(e => e.NumeroInscripcion <= int.Parse(escrituraViewModel.Escritura.NumeroInscripcion))
                        .ToList();

                    if (multipropietarioEqualYear.Count > 0)
                    {
                        foreach (var equalYear in multipropietarioEqualYear)
                        {
                            _context.Remove(equalYear);
                        }
                    }

                    var multipropietario = new Multipropietario
                    {
                        Comuna = escrituraViewModel.Escritura.Comuna,
                        Manzana = int.Parse(escrituraViewModel.Escritura.Manzana),
                        Predio = int.Parse(escrituraViewModel.Escritura.Predio),
                        Fojas = escrituraViewModel.Escritura.Fojas,
                        FechaInscripcion = preYear,
                        NumeroInscripcion = int.Parse(escrituraViewModel.Escritura.NumeroInscripcion),
                        RunRut = AdquirienteRun[i],
                        AnoInscripcion = int.Parse(preYear.Year.ToString()),
                        AnoVigenciaInicial = int.Parse(preYear.Year.ToString()),
                        AnoVigenciaFinal = 0,
                        PorcentajeDerecho = double.Parse(AdquirentePorcentajeDerecho[i]) + promedioPorcentajeDerecho
                    };

                    _context.Add(multipropietario);
                }


                _context.Escrituras.Add(escritura);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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
