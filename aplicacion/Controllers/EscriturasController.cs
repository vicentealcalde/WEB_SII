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

        public static List<string> InsertZeros(List<string> PorcentajeDerechoNoAcreditado, List<string> PorcentajeDerecho)
        {
            List<string> PorcentajeDerechoModificado = new List<string>();
            int contador = 0;
            
            for (int i = 0; i < PorcentajeDerechoNoAcreditado.Count; i++)
            {
                if (PorcentajeDerechoNoAcreditado[i].ToLower() == "true")
                {
                    PorcentajeDerechoModificado.Add("0");
                }
                else
                {
                    if (contador >= PorcentajeDerecho.Count)
                    {
                        break;
                    }
                    PorcentajeDerechoModificado.Add(PorcentajeDerecho[contador]);
                    contador++;
                }
            }
            
            return PorcentajeDerechoModificado;
        }

        public void ActualizarAnoVigenciaFinal(List<Multipropietario> multipropietarios, int nuevoAnoVigenciaFinal)
        {
            foreach (var multipropietario in multipropietarios)
            {
                if (multipropietario.AnoVigenciaInicial <= nuevoAnoVigenciaFinal)
                {
                    multipropietario.AnoVigenciaFinal = nuevoAnoVigenciaFinal;
                }
                else{
                    multipropietario.AnoVigenciaFinal = multipropietario.AnoVigenciaInicial;
                }
                
            }

            _context.SaveChanges();
        }
        public List<Multipropietario> ObtenerMultipropietarios(List<string> Ruts, int manzana, int predio, string comuna )
        {
            var mp = _context.Multipropietarios
            .Where(m => m.Manzana == manzana && m.Predio == predio && m.Comuna == comuna && m.AnoVigenciaFinal == 0)
            .ToList();
            List<Multipropietario> listMultipropietarios = new List<Multipropietario>();
            foreach (string rut in Ruts)
            {
                Multipropietario multipropietario = mp.FirstOrDefault(mp => mp.RunRut == rut);
                if (multipropietario != null)
                {
                    listMultipropietarios.Add(multipropietario);
                }
            }

            return listMultipropietarios;
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

        public List<Multipropietario> ActualizarPorcentajes(List<Multipropietario> multipropietarios, List<string> porcentajes, Escritura escritura)
        {
            List<Multipropietario> nuevosMultipropietarios = new List<Multipropietario>();
            
            // Verificar que la cantidad de porcentajes coincida con la cantidad de multipropietarios
            if (multipropietarios.Count != porcentajes.Count)
            {
                throw new ArgumentException("La cantidad de porcentajes no coincide con la cantidad de multipropietarios.");
            }

            // Crear nuevos multipropietarios con los mismos datos que los originales, pero con PorcentajeDerecho modificado
            for (int i = 0; i < multipropietarios.Count; i++)
            {
                // Convertir porcentaje a decimal
                decimal porcentaje = decimal.Parse(porcentajes[i]);

                // Crear nuevo multipropietario con datos del original
                Multipropietario nuevoMultipropietario = new Multipropietario
                {
                    Comuna = escritura.Comuna,
                    Manzana = int.Parse(escritura.Manzana),
                    Predio = int.Parse(escritura.Predio),
                    Fojas = escritura.Fojas,
                    FechaInscripcion = escritura.FechaInscripcion,
                    NumeroInscripcion = int.Parse(escritura.NumeroInscripcion),   
                    AnoInscripcion = escritura.FechaInscripcion.Year,
                    AnoVigenciaInicial = escritura.FechaInscripcion.Year,  
                    RunRut = multipropietarios[i].RunRut,
                    PorcentajeDerecho = multipropietarios[i].PorcentajeDerecho -multipropietarios[i].PorcentajeDerecho*(100 - (double)porcentaje)/100,
                    AnoVigenciaFinal = 0 // AnoVigenciaFinal se establece en cero
                };

                // Agregar nuevo multipropietario a la lista
                nuevosMultipropietarios.Add(nuevoMultipropietario);
            }

            return nuevosMultipropietarios;
        }

         public List<Multipropietario> ActualizarPorcentajesDominio(List<Multipropietario> multipropietarios, List<string> porcentajes, Escritura escritura)
        {
            List<Multipropietario> nuevosMultipropietarios = new List<Multipropietario>();
            
            // Verificar que la cantidad de porcentajes coincida con la cantidad de multipropietarios
            if (multipropietarios.Count != porcentajes.Count)
            {
                throw new ArgumentException("La cantidad de porcentajes no coincide con la cantidad de multipropietarios.");
            }

            // Crear nuevos multipropietarios con los mismos datos que los originales, pero con PorcentajeDerecho modificado
            for (int i = 0; i < multipropietarios.Count; i++)
            {
                // Convertir porcentaje a decimal
                decimal porcentaje = decimal.Parse(porcentajes[i]);
                var NewPercent = 0.0;
                if (multipropietarios[i].PorcentajeDerecho > (double)porcentaje)
                {
                    NewPercent = multipropietarios[i].PorcentajeDerecho - (double)porcentaje;
                }

                // Crear nuevo multipropietario con datos del original
                Multipropietario nuevoMultipropietario = new Multipropietario
                {
                    Comuna = escritura.Comuna,
                    Manzana = int.Parse(escritura.Manzana),
                    Predio = int.Parse(escritura.Predio),
                    Fojas = escritura.Fojas,
                    FechaInscripcion = escritura.FechaInscripcion,
                    NumeroInscripcion = int.Parse(escritura.NumeroInscripcion),   
                    AnoInscripcion = escritura.FechaInscripcion.Year,
                    AnoVigenciaInicial = escritura.FechaInscripcion.Year,  
                    RunRut = multipropietarios[i].RunRut,
                    PorcentajeDerecho = NewPercent,
                    AnoVigenciaFinal = 0 // AnoVigenciaFinal se establece en cero
                };

                // Agregar nuevo multipropietario a la lista
                nuevosMultipropietarios.Add(nuevoMultipropietario);
            }

            return nuevosMultipropietarios;
        }

        public bool VerificarPropiedad(List<string> ruts, string comuna, int manzana, int predio)
        {
            
            var mp = _context.Multipropietarios
            .Where(m => m.Manzana == manzana && m.Predio == predio && m.Comuna == comuna && m.AnoVigenciaFinal == 0)
            .ToList();
            foreach (string rut in ruts)
            {
                
                if (!mp.Any(m => m.RunRut == rut))
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
                var EnajenateRun = (Request.Form["Enajenante.RunRut"].ToString()).Split(",");
                var EnajenantePorcentajeDerecho = (Request.Form["Enajenante.PorcentajeDerecho"].ToString()).Split(",");
                var EnajenantePorcentajeDerechoNoAcreditado = (Request.Form["Enajenante.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
                var AdquirienteRun = (Request.Form["Adquirente.RunRut"].ToString()).Split(",");
                var AdquirentePorcentajeDerecho = (Request.Form["Adquirente.PorcentajeDerecho"].ToString()).Split(",");
                
                var AdquirentePorcentajeDerechoNoAcreditado = (Request.Form["Adquirente.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
                var IsValidEnajenantes = VerificarPropiedad(
                    EnajenateRun.ToList(),
                    escrituraViewModel.Escritura.Comuna,
                    int.Parse(escrituraViewModel.Escritura.Manzana),
                    int.Parse(escrituraViewModel.Escritura.Predio)
                    );

                if (IsValidEnajenantes == false){
                    return Create();
                }
                AdquirentePorcentajeDerecho = InsertZeros(AdquirentePorcentajeDerechoNoAcreditado.ToList(), AdquirentePorcentajeDerecho.ToList()).ToArray();
               
                if (AdquirentePorcentajeDerecho.Length < AdquirienteRun.Length)
                {
                    var AdquirentePorcentajeDerechoModificada = AdquirentePorcentajeDerecho.ToList();
                    var AdquirentePorcentajeDerechoNoAcreditadoModi = AdquirentePorcentajeDerechoNoAcreditado.ToList();
                    for (int i = 0; i < AdquirienteRun.Length - AdquirentePorcentajeDerecho.Length; i++)
                    {

                        AdquirentePorcentajeDerechoModificada.Add("0");
                        AdquirentePorcentajeDerechoNoAcreditadoModi.Add("true");
                    }
                    AdquirentePorcentajeDerecho = AdquirentePorcentajeDerechoModificada.ToArray();
                    AdquirentePorcentajeDerechoNoAcreditado = AdquirentePorcentajeDerechoNoAcreditadoModi.ToArray();
                }
                AdquirentePorcentajeDerecho = ProcesarPorcentajes(AdquirentePorcentajeDerecho.ToList()).ToArray();
                var DifSumPercentAdquiriente = DIfSumPercent(AdquirentePorcentajeDerecho.ToList());
                var DifSumPercentEnajenate = DIfSumPercent(EnajenantePorcentajeDerecho.ToList());
                if (EnajenateRun.Count() == 1 &&  AdquirienteRun.Count() == 1 )
                {
                    if (DifSumPercentAdquiriente != 0 && DifSumPercentAdquiriente != 100)
                    {
                        List<Multipropietario> DataEnajenante = ObtenerMultipropietarios(
                            EnajenateRun.ToList(),
                            int.Parse(escrituraViewModel.Escritura.Manzana),
                            int.Parse(escrituraViewModel.Escritura.Predio),
                            escrituraViewModel.Escritura.Comuna
                            );
                        ActualizarAnoVigenciaFinal(DataEnajenante, escritura.FechaInscripcion.Year - 1);
                        EnajenantePorcentajeDerecho[0] = (100 - double.Parse(EnajenantePorcentajeDerecho[0])).ToString() ;
                        List<Multipropietario> NewEnajenantes = ActualizarPorcentajes(DataEnajenante,EnajenantePorcentajeDerecho.ToList(),escritura);


                        var Newmultipropietario = new Multipropietario
                        {
                            Comuna = escrituraViewModel.Escritura.Comuna,
                            Manzana = int.Parse(escrituraViewModel.Escritura.Manzana),
                            Predio = int.Parse(escrituraViewModel.Escritura.Predio),
                            Fojas = escrituraViewModel.Escritura.Fojas,
                            FechaInscripcion = escritura.FechaInscripcion,
                            NumeroInscripcion = int.Parse(escrituraViewModel.Escritura.NumeroInscripcion),
                            RunRut = AdquirienteRun[0],
                            AnoInscripcion = escritura.FechaInscripcion.Year,
                            AnoVigenciaInicial = escritura.FechaInscripcion.Year,
                            AnoVigenciaFinal = 0,
                            PorcentajeDerecho = (
                                DataEnajenante[0].PorcentajeDerecho*
                                double.Parse(AdquirentePorcentajeDerecho[0])/100)
                        };
                        _context.Add(NewEnajenantes[0]);
                        _context.Add(Newmultipropietario);
                    }
                    else {
                        List<Multipropietario> DataEnajenante = ObtenerMultipropietarios(
                            EnajenateRun.ToList(),
                            int.Parse(escrituraViewModel.Escritura.Manzana),
                            int.Parse(escrituraViewModel.Escritura.Predio),
                            escrituraViewModel.Escritura.Comuna
                            );
                        ActualizarAnoVigenciaFinal(DataEnajenante, escritura.FechaInscripcion.Year - 1);
                        var Newmultipropietario = new Multipropietario
                        {
                            Comuna = escrituraViewModel.Escritura.Comuna,
                            Manzana = int.Parse(escrituraViewModel.Escritura.Manzana),
                            Predio = int.Parse(escrituraViewModel.Escritura.Predio),
                            Fojas = escrituraViewModel.Escritura.Fojas,
                            FechaInscripcion = escritura.FechaInscripcion,
                            NumeroInscripcion = int.Parse(escrituraViewModel.Escritura.NumeroInscripcion),
                            RunRut = AdquirienteRun[0],
                            AnoInscripcion = escritura.FechaInscripcion.Year,
                            AnoVigenciaInicial = escritura.FechaInscripcion.Year,
                            AnoVigenciaFinal = 0,
                            PorcentajeDerecho = (
                                DataEnajenante[0].PorcentajeDerecho*
                                double.Parse(AdquirentePorcentajeDerecho[0])/100)
                        };
                        _context.Add(Newmultipropietario);
                        
                    }

                }
                else{
                    if (DifSumPercentAdquiriente == 0 || DifSumPercentAdquiriente == 100 )  //Los enajenantes pierden el porcentaje de la propiedad este es 1 vs 1
                    {
                        List<Multipropietario> DataEnajenante = ObtenerMultipropietarios(
                            EnajenateRun.ToList(),
                            int.Parse(escrituraViewModel.Escritura.Manzana),
                            int.Parse(escrituraViewModel.Escritura.Predio),
                            escrituraViewModel.Escritura.Comuna
                            );
                        ActualizarAnoVigenciaFinal(DataEnajenante, escritura.FechaInscripcion.Year - 1);
                        var sumaPorcentajeDerecho = DataEnajenante.Sum(m => m.PorcentajeDerecho);
                        for (int nextAdquiriente = 0; nextAdquiriente < AdquirienteRun.Length; nextAdquiriente ++)
                        {
                            //Con esto creo el nuevo adquiriente 
                            var Newmultipropietario = new Multipropietario
                                {
                                    Comuna = escrituraViewModel.Escritura.Comuna,
                                    Manzana = int.Parse(escrituraViewModel.Escritura.Manzana),
                                    Predio = int.Parse(escrituraViewModel.Escritura.Predio),
                                    Fojas = escrituraViewModel.Escritura.Fojas,
                                    FechaInscripcion = escritura.FechaInscripcion,
                                    NumeroInscripcion = int.Parse(escrituraViewModel.Escritura.NumeroInscripcion),
                                    RunRut = AdquirienteRun[nextAdquiriente],
                                    AnoInscripcion = escritura.FechaInscripcion.Year,
                                    AnoVigenciaInicial = escritura.FechaInscripcion.Year,
                                    AnoVigenciaFinal = 0,
                                    PorcentajeDerecho = (
                                        sumaPorcentajeDerecho*
                                        double.Parse(AdquirentePorcentajeDerecho[nextAdquiriente])/100)
                                };

                            _context.Add(Newmultipropietario);
                        }
                        
                            //ActualizarPorcentajes();  

                    }
                    else
                    {
                        List<Multipropietario> DataEnajenante = ObtenerMultipropietarios(
                            EnajenateRun.ToList(),
                            int.Parse(escrituraViewModel.Escritura.Manzana),
                            int.Parse(escrituraViewModel.Escritura.Predio),
                            escrituraViewModel.Escritura.Comuna
                            );
                        ActualizarAnoVigenciaFinal(DataEnajenante, escritura.FechaInscripcion.Year - 1);
                        List<Multipropietario> NewEnajenantes = ActualizarPorcentajesDominio(DataEnajenante,EnajenantePorcentajeDerecho.ToList(),escritura);
                        var SumPercentRepar = EnajenantePorcentajeDerecho.ToList().Sum(n => int.Parse(n));

                        foreach (Multipropietario NewEnajenate in NewEnajenantes)
                        {
                            _context.Add(NewEnajenate);
                        }

                        for (int nextAdquiriente = 0; nextAdquiriente < AdquirienteRun.Length; nextAdquiriente ++)
                        {
                            //Con esto creo el nuevo adquiriente 
                            var Newmultipropietario = new Multipropietario
                                {
                                    Comuna = escrituraViewModel.Escritura.Comuna,
                                    Manzana = int.Parse(escrituraViewModel.Escritura.Manzana),
                                    Predio = int.Parse(escrituraViewModel.Escritura.Predio),
                                    Fojas = escrituraViewModel.Escritura.Fojas,
                                    FechaInscripcion = escritura.FechaInscripcion,
                                    NumeroInscripcion = int.Parse(escrituraViewModel.Escritura.NumeroInscripcion),
                                    RunRut = AdquirienteRun[nextAdquiriente],
                                    AnoInscripcion = escritura.FechaInscripcion.Year,
                                    AnoVigenciaInicial = escritura.FechaInscripcion.Year,
                                    AnoVigenciaFinal = 0,
                                    PorcentajeDerecho = (
                                        double.Parse(AdquirentePorcentajeDerecho[nextAdquiriente])/100)
                                };

                            _context.Add(Newmultipropietario);
                        }

                    }
                
                }



            }

            if (escritura.Cne == "regularizacion")
            {
                var AdquirienteRun = (Request.Form["Adquirente.RunRut"].ToString()).Split(",");
                var AdquirentePorcentajeDerecho = (Request.Form["Adquirente.PorcentajeDerecho"].ToString()).Split(",");
                var AdquirentePorcentajeDerechoNoAcreditado = (Request.Form["Adquirente.PorcentajeDerechoNoAcreditado"].ToString()).Split(",");
                
                AdquirentePorcentajeDerecho = InsertZeros(AdquirentePorcentajeDerechoNoAcreditado.ToList(), AdquirentePorcentajeDerecho.ToList()).ToArray();
               
                if (AdquirentePorcentajeDerecho.Length < AdquirienteRun.Length)
                {
                    var AdquirentePorcentajeDerechoModificada = AdquirentePorcentajeDerecho.ToList();
                    var AdquirentePorcentajeDerechoNoAcreditadoModi = AdquirentePorcentajeDerechoNoAcreditado.ToList();
                    for (int i = 0; i < AdquirienteRun.Length - AdquirentePorcentajeDerecho.Length; i++)
                    {

                        AdquirentePorcentajeDerechoModificada.Add("0");
                        AdquirentePorcentajeDerechoNoAcreditadoModi.Add("true");
                    }
                    AdquirentePorcentajeDerecho = AdquirentePorcentajeDerechoModificada.ToArray();
                    AdquirentePorcentajeDerechoNoAcreditado = AdquirentePorcentajeDerechoNoAcreditadoModi.ToArray();
                }
                AdquirentePorcentajeDerecho = ProcesarPorcentajes(AdquirentePorcentajeDerecho.ToList()).ToArray();

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
                        PorcentajeDerecho = double.Parse(AdquirentePorcentajeDerecho[i]) 
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
