using System.ComponentModel.DataAnnotations;

namespace Uttt.Micro.Service.Modelo
{
    public class LibreriaMaterial
    {
        [Key]
        public Guid? LibreriaMateriaId { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? AutorLibro { get; set; }
        public int NewData { get; set; }
    }
}