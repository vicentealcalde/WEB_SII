using System.Collections.Generic;
using aplicacion.models;


namespace aplicacion.ViewModels;

public class EscrituraViewModel
{
    public Escritura Escritura { get; set; }

    public Adquirente Adquirente { get; set; }
    public Enajenante Enajenante { get; set; }
    public List<Adquirente> Adquirentes { get; set; }
    public List<Enajenante> Enajenantes { get; set; }

    public List<Comuna> Comunas { get; set; }


}

public class Comuna
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }
}