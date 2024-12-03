using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpinionesPeliculasPWA.Models.Dtos;
using OpinionesPeliculasPWA.Models.Entities;
using OpinionesPeliculasPWA.Repositories;

namespace OpinionesPeliculasPWA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly Repository<Peliculas> repository;
        public HomeController(Repository<Peliculas> repository)
        {
            this.repository = repository;
        }
        public IActionResult Get()
        {
            var pelis = repository.GetAll();
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
                return Ok(pelis);
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
                // falta agregar la imagen de la pelicula ponganlo en wwwroot mancos
                Peliculas peli = new()
                {
                    Nombre = dto.Title
                };
                repository.Insert(peli);
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
