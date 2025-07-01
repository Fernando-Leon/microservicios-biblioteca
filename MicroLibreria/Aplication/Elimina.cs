using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Service.Persistencia;

namespace Uttt.Micro.Service.Aplication
{
    public class Elimina
    {
        public class EliminaLibro : IRequest
        {
            public Guid? LibroId { get; set; }
        }

        public class Manejador : IRequestHandler<EliminaLibro>
        {
            private readonly ContextoLibreria _contexto;
            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(EliminaLibro request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriasMateriales
                    .Where(x => x.LibreriaMateriaId == request.LibroId).FirstOrDefaultAsync();
                if (libro == null)
                {
                    throw new Exception("No se encontro el libro");
                }
                _contexto.LibreriasMateriales.Remove(libro);
                var valor = await _contexto.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el libro");
            }
        }
    }
}
