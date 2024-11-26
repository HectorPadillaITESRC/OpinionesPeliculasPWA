using System;
using System.Collections.Generic;

namespace OpinionesPeliculasPWA.Models.Entities;

public partial class Tokens
{
    public int Id { get; set; }

    public int IdUsuario { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Timestamp { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
