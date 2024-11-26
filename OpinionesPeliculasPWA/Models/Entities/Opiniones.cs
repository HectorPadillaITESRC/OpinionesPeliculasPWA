using System;
using System.Collections.Generic;

namespace OpinionesPeliculasPWA.Models.Entities;

public partial class Opiniones
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string IdPelicula { get; set; } = null!;

    public string Opinion { get; set; } = null!;
}
