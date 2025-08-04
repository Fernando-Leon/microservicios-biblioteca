using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda.Microservicios.Autor.Api.Aplication;

namespace Tienda.Microservicios.Autor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requiere JWT para acceder a los endpoints
    public class AutorController: ControllerBase
    {
        private readonly IMediator _mediator;
        public AutorController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _mediator.Send(new Consulta.ListaAutor());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDto>> GetAutorLibro(string id)
        {
            return await _mediator.Send(new ConsultarFiltro.AutorUnico { AutorGuid = id });
        }

        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<AutorDto>> GetAutorPorNombre(string nombre)
        {
            try
            {
                return await _mediator.Send(new ConsultarFiltro.AutorUnico { Nombre = nombre });
            }
            catch (Exception ex)
            {
                if (ex.Message == "No se encontró el autor")
                {
                    return NotFound();
                }
                throw;
            }
        }

        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            var userName = User.Identity?.Name;
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new 
            { 
                message = "Acceso autorizado a Microservicio de Autores", 
                usuario = userName,
                claims = userClaims
            });
        }

        [HttpGet("test-simple")]
        public IActionResult TestSimple()
        {
            return Ok(new { message = "Autenticación JWT funcionando correctamente", timestamp = DateTime.Now });
        }

    }
}
