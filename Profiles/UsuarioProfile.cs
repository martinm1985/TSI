using System.Linq;
using Crud.DTOs;
using Crud.Models;
using AutoMapper;

namespace Crud.Profiles
{
  public class UserProfile : Profile {
    public UserProfile () {
      CreateMap<Creador, CreadorData>()
                .ForMember(data => data.Categorias, model => model.MapFrom(src => new string[] {
                  src.Categoria1 == null ? src.Categoria1.Nombre : null,
                  src.Categoria2 == null ? src.Categoria2.Nombre : null
                }.Where(v => v!=null )));

      CreateMap<User, UserData>().ForMember(data => data.Creador, model => model.MapFrom(src => src.Creador));
    }
  }
}
