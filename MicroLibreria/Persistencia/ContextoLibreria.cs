using Microsoft.EntityFrameworkCore;
using Uttt.Micro.Service.Modelo;

namespace Uttt.Micro.Service.Persistencia
{
    public class ContextoLibreria: DbContext
    {
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options)
        {
        }

        public DbSet<LibreriaMaterial> LibreriasMateriales { get; set; }
    }
}
