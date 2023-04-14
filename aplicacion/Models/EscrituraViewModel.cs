using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using aplicacion.models;
using MessagePack;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;


namespace aplicacion.ViewModels;

public class EscrituraViewModel
{
    public Escritura Escritura { get; set; }

    public Adquirente Adquirente { get; set; }
    public Enajenante Enajenante { get; set; }
    public List<Adquirente> Adquirentes { get; set; }
    public List<Enajenante> Enajenantes { get; set; }
    public List<string> Comunas { get; set; }
}

