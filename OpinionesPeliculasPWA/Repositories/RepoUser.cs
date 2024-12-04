using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpinionesPeliculasPWA.Models.Entities;

namespace OpinionesPeliculasPWA.Repositories
{
    public class RepoUser: Repository<Usuarios>
    {
        public RepoUser(OpinionespeliculasContext context) : base(context)
        {
            
        }

        public Usuarios? GetUserByEmail(string email){
            var user = _dbSet.Where(x=>x.Correo == email).FirstOrDefault();

            return user;
        }

        public Usuarios? GetUserByRefreshToken (string refreshToken){
            var user = _context.Set<Tokens>().Include(x=> x.IdUsuarioNavigation).Where(x=> x.Token == refreshToken).Select(x=>x.IdUsuarioNavigation).FirstOrDefault();

            return user;

        }
        
    }
}