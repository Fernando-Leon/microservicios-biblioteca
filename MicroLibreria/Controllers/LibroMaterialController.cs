using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uttt.Micro.Service.Aplication;

namespace Uttt.Micro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requiere JWT para acceder a los endpoints
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LibroMaterialController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibroUnico(Guid id)
        {
            return await _mediator.Send(new ConsultaFiltro.LibroUnico
            {
                LibroId = id
            });
        }

        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            var userName = User.Identity?.Name;
            var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new 
            { 
                message = "Acceso autorizado a MicroLibreria", 
                usuario = userName,
                claims = userClaims
            });
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Unit>> Eliminar(Guid id)
        //{
        //    await _mediator.Send(new Elimina.EliminaLibro
        //    {
        //        LibroId = id
        //    });

        //    return NoContent();
        //}
    }
}
