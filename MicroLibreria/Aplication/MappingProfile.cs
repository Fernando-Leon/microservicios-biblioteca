using AutoMapper;
using Uttt.Micro.Service.Modelo;

namespace Uttt.Micro.Service.Aplication
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        { 
            CreateMap<LibreriaMaterial, LibroMaterialDto>();
        }
    }
}
