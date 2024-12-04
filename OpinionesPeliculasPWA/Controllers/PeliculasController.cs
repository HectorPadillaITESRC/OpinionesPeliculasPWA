using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop.Implementation;
using OpinionesPeliculasPWA.Helpers;
using OpinionesPeliculasPWA.Models.Dtos;
using OpinionesPeliculasPWA.Models.Entities;
using OpinionesPeliculasPWA.Repositories;

namespace OpinionesPeliculasPWA.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly Repository<Peliculas> repository;
        public PeliculasController(Repository<Peliculas> repository)
        {
            this.repository = repository;
        }
        public IActionResult Get()
        {
            var pelis = repository.GetAll().Select(x=> new MovieDto
            {
                Id=x.Id,
                Portada = ImageToBase64.ConvertBase64($"wwwroot/images/{x.Id}.jpg"),
                Title = x.Nombre
            });
            if (pelis != null)
            {

                return Ok(pelis);
            }
            return BadRequest("No hay peliculas");
        }

        public IActionResult Get(int id)
        {
            var pelis = repository.Get(id);

            if (pelis != null)
            {
                var pelidto = new MovieDto
                {
                    Id = pelis.Id,
                    Portada = ImageToBase64.ConvertBase64($"wwwroot/images/{pelis.Id}.jpg"),
                    Title = pelis.Nombre
                };
                return Ok();
            }
            return BadRequest("No hay peliculas");
        }
        [HttpPost]
        public IActionResult Post(MovieDto dto)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(dto.Title))
                {
                    return BadRequest("Ingrese un titulo");
                }
               
                Peliculas peli = new()
                {
                    Nombre = dto.Title
                };
                repository.Insert(peli);
                if (string.IsNullOrEmpty(dto.Portada))
                {
                    System.IO.File.Copy("wwwroot/images/Default.png", $"wwwroot/images/{peli.Id}.jpg");
                }
                else
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{peli.Id}.jpg");
                    var bytes = Convert.FromBase64String(dto.Portada);
                    System.IO.File.WriteAllBytes(path, bytes);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public IActionResult Put(MovieDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Title))
                {
                    return BadRequest("Ingrese el titulo");
                }
                var peli = repository.Get(dto.Id);
                if (peli != null)
                {
                    peli.Nombre = dto.Title;
                    repository.Update(peli);
                    if (string.IsNullOrEmpty(dto.Portada))
                    {
                        System.IO.File.Copy("wwwroot/images/Default.jpg", $"wwwroot/images/{peli.Id}.jpg");
                    }
                    else
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{peli.Id}.jpg");
                        var bytes = Convert.FromBase64String(dto.Portada);
                        System.IO.File.WriteAllBytes(path, bytes);
                    }
                    return Ok();
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var peli = repository.Get(id);
                if (peli != null)
                {
                    repository.Delete(id);
                    return Ok();
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
