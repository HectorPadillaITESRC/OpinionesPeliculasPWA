using System;
using System.Collections.Generic;

namespace OpinionesPeliculasPWA.Models.Entities;

public partial class Peliculas
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;
}
