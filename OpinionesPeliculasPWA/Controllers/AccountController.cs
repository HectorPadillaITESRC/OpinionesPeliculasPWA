using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpinionesPeliculasPWA.Helpers;
using OpinionesPeliculasPWA.Models.DTOs;
using OpinionesPeliculasPWA.Models.Entities;
using OpinionesPeliculasPWA.Repositories;

namespace OpinionesPeliculasPWA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private readonly RepoUser repoUser;
        private readonly Repository<Usuarios> repositoryUsuarios;
        private readonly TokenGenerator tokenGenerator;
        private readonly Repository<Tokens> repositoryTokens;
        public AccountController(Repository<Usuarios> repositoryUsuarios, 
        TokenGenerator tokenGenerator, 
        Repository<Tokens> repositoryTokens,
        RepoUser repoUser)
        {
            this.repoUser = repoUser;
            this.repositoryUsuarios = repositoryUsuarios;
            this.tokenGenerator = tokenGenerator;
            this.repositoryTokens = repositoryTokens;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto){
            try{
                if(string.IsNullOrEmpty(dto.Email)){return BadRequest("Ingrese un correo valido. ");}
                if(string.IsNullOrEmpty(dto.Password)) {return BadRequest("Ingrese una contraseña valida. ");}

                var user = repoUser.GetUserByEmail(dto.Email);
                if(user == null) { return BadRequest(); }
                
                var refreshToken = tokenGenerator.GenerateRefresh(user);

                var tokenEntity = new Tokens
                {
                    IdUsuario = user.Id,
                    Token = refreshToken,
                    Timestamp = DateTime.Now
                };

                repositoryTokens.Insert(tokenEntity);

                var dat = new { token = tokenGenerator.Generate(user), refresh = refreshToken };

                var cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(20),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                    Path = "/",
                
                };

                Response.Cookies.Append("jwt", refreshToken, cookie);

                return Ok(dat);

            }
            catch(Exception ex){
                return Problem(ex.Message);
            }
        }

        [HttpPost("signup")]
        public IActionResult SignUp(SignUpDTO signUpDTO)
        {
            if (string.IsNullOrWhiteSpace(signUpDTO.Email))
            {
                return BadRequest("El correo electrónico no puede estar vacío");
            }
            if (string.IsNullOrWhiteSpace(signUpDTO.Password))
            {
                return BadRequest("La contraseña no puede estar vacía");
            }
            if (string.IsNullOrWhiteSpace(signUpDTO.VerifiedPassword))
            {
                return BadRequest("La contraseña no puede estar vacía");
            }
            if (signUpDTO.Password != signUpDTO.VerifiedPassword)
            {
                return BadRequest("Las contraseñas no coinciden");
            }

            var usr = repositoryUsuarios.GetAll().FirstOrDefault(x => x.Correo == signUpDTO.Email);
            if (usr != null)
            {
                return BadRequest("El correo electrónico ya está registrado");
            }
            // Encriptar contrasseña

            var usuario = new Usuarios
            {
                Correo = signUpDTO.Email,
                Contrasena = Encrypter.ConvertToSHA256(signUpDTO.Password),
                // Revisar rol del nuevo usuario
                IdRol = UsuarioRol.RolUsuario
            };

            repositoryUsuarios.Insert(usuario);

            var refreshToken = tokenGenerator.GenerateRefresh(usuario);

            var tokenEntity = new Tokens
            {
                IdUsuario = usuario.Id,
                Token = refreshToken,
                Timestamp = DateTime.Now
            };

            repositoryTokens.Insert(tokenEntity);

            var dat = new { token = tokenGenerator.Generate(usuario), refresh = refreshToken };

             var cookie = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(20),
                HttpOnly = true,
                Secure = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
                Path = "/",
               
            };

            Response.Cookies.Append("jwt", refreshToken, cookie);

            return Ok(dat);

        }

        [HttpPost]
        public IActionResult Refresh(){
            try{
                Request.Cookies.TryGetValue("jwt", out string? token);
                if(token == null) { return Unauthorized(); }
                var user = repoUser.GetUserByRefreshToken(token);
                if(user == null) { return Unauthorized(); }

                var dat = new { token = tokenGenerator.Generate(user), refresh = token };

                return Ok(dat);
            }
            catch(Exception ex){
                return Problem(ex.Message);
            }
        }
    }
}