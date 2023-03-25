using System;
using System.Collections.Generic;

namespace aplicacion.models;

public partial class Escritura
{
    public int NumAtencion { get; set; }

    public string Cne { get; set; } = null!;

    public string Comuna { get; set; } = null!;

    public string Manzana { get; set; } = null!;

    public string Predio { get; set; } = null!;

    public int Fojas { get; set; }

    public DateTime FechaInscripcion { get; set; }

    public string NumeroInscripcion { get; set; } = null!;

    public virtual ICollection<Adquirente> Adquirentes { get; } = new List<Adquirente>();

    public virtual ICollection<Enajenante> Enajenantes { get; } = new List<Enajenante>();
}
