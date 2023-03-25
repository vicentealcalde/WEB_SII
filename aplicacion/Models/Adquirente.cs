using System;
using System.Collections.Generic;

namespace aplicacion.models;

public partial class Adquirente
{
    public int IdAdquirente { get; set; }

    public int NumAtencion { get; set; }

    public string RunRut { get; set; } = null!;

    public double PorcentajeDerecho { get; set; }

    public bool PorcentajeDerechoNoAcreditado { get; set; }

    public virtual Escritura NumAtencionNavigation { get; set; } = null!;
}
