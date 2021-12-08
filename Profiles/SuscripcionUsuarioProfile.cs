using AutoMapper;
using Crud.Models;

namespace Crud.DTOs
{
    public class SuscripcionUsuarioProfile : Profile
    {
        public SuscripcionUsuarioProfile()
        {
            CreateMap<SuscripcionUsuario, SuscripcionUsuarioDto>();
        }
    }
}
