using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpinionesPeliculasPWA.Models.DTOs
{
    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}