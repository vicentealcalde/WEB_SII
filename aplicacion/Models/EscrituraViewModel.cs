using System.Collections.Generic;
using aplicacion.models;


namespace aplicacion.ViewModels;

public class EscrituraViewModel
{
    public Escritura Escritura { get; set; }
    public List<Adquirente> Adquirentes { get; set; }
    public List<Enajenante> Enajenantes { get; set; }

    public EscrituraViewModel()
    {
        Adquirentes = new List<Adquirente>() { new Adquirente() };
        Enajenantes = new List<Enajenante>() { new Enajenante() };
    }

}