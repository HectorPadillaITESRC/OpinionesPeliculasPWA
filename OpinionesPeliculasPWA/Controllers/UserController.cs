using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpinionesPeliculasPWA.Helpers;
using OpinionesPeliculasPWA.Repositories;

namespace OpinionesPeliculasPWA.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "1")]
    public class UserController : Controller
    {
        private readonly RepoUser repoUser;
        public UserController(RepoUser repoUser)
        {
            this.repoUser = repoUser;
        }

        [HttpGet]
        public IActionResult GetAllUsers(){
            try{
                var users = repoUser.GetAll().Select(x=> x.Correo);

                if(users == null){return BadRequest("No hay usuarios registrados. ");}
                return Ok(users);
            }
            catch(Exception ex){
                return Problem(ex.Message);
            }
        }	
    }
}