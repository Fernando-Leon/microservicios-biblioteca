using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uttt.Micro.Service.Aplication;

namespace Uttt.Micro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
