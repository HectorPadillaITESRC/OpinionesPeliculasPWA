using System;
using System.Collections.Generic;

namespace OpinionesPeliculasPWA.Models.Entities;

public partial class Usuarios
{
    public int Id { get; set; }

    public string Contrasena { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual ICollection<Tokens> Tokens { get; set; } = new List<Tokens>();
}
