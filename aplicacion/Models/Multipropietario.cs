using System;
using System.Collections.Generic;

namespace aplicacion.models;

public partial class Multipropietario
{
    public int Id { get; set; }

    public string Comuna { get; set; } = null!;

    public int Manzana { get; set; }

    public int Predio { get; set; }

    public string RunRut { get; set; } = null!;

    public double PorcentajeDerecho { get; set; }

    public int Fojas { get; set; }

    public int AnoInscripcion { get; set; }

    public int NumeroInscripcion { get; set; }

    public DateTime FechaInscripcion { get; set; }

    public int AnoVigenciaInicial { get; set; }

    public int AnoVigenciaFinal { get; set; }
}
