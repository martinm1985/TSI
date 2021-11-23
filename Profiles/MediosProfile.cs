using System.Linq;
using Crud.DTOs;
using Crud.Models;
using AutoMapper;

namespace Crud.Profiles
{
    public class TarjetasProfile : Profile
    {
        public TarjetasProfile()
        {
            CreateMap<Tarjeta, TarjetaResponse>()
                      .ForMember(data => data.EntidadFinanciera, model => model.MapFrom(src => src.EntidadFinanciera.Nombre));
        }
    }

    public class CuentasProfile : Profile
    {
        public CuentasProfile()
        {
            CreateMap<Cuenta, CuentaResponse>()
                      .ForMember(data => data.EntidadFinanciera, model => model.MapFrom(src => src.EntidadFinanciera.Nombre));
        }
    }
}
