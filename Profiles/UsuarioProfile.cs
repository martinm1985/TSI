using System.Linq;
using Crud.DTOs;
using Crud.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using static Crud.DTOs.SuscripcionDto;

namespace Crud.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<TipoSuscripcion, TipoSuscripcionDto>();
            CreateMap<Creador, CreadorData>()
                      .ForMember(data => data.Categorias, model => model.MapFrom(src => new Categoria[] {
                  src.Categoria1,
                  src.Categoria2
                      }.Where(v => v != null)))
                      .ForMember(data => data.TiposDeSuscripciones, model => model.MapFrom(src => src.TiposDeSuscripciones));

            CreateMap<User, UserData>()
                      .ForMember(data => data.Creador, model => model.MapFrom(src => src.Creador));
        }
    }
}
