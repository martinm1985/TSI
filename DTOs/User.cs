using Crud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Crud.DTOs.SuscripcionDto;

namespace Crud.DTOs
{
    public record LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public record UserRegister
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

    }

    public class ResponseRegistrationSuccess
    {
        public UserData User {get; set;}
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }

    public record ResponseFailure
    {
        public string Error { get; set; }
    }

    public record RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UserData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool isAdministrador { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public int IdTipoSucripcionUsuario { get; set; }

        public CreadorData Creador {get; set;} 

        public FileDto.File Imagen { get; set; }
        public FileDto.File ImagePortada { get; set; }
    }

    public record CreadorRegister
    {
        public string Descripcion { get; set; }
        public FileDto.File Imagen { get; set; }
        public FileDto.File ImagePortada { get; set; }
        public string Biografia { get; set; }
        public string VideoYoutube { get; set; }
        public string MsjBienvenidaGral { get; set; }
        public int Categoria1Id { get; set; }
        public int Categoria2Id { get; set; }
        public string UserId { get; set; }

    }

    public record CreadorBasicData
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Imagen { get; set; }
        public string Descripcion { get; set; }
        public Categoria Categoria1 { get; set; }
        public Categoria Categoria2 { get; set; }

    }

    public record CreadorData
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string ImagePortada { get; set; }
        public string Biografia { get; set; }
        public string VideoYoutube { get; set; }
        public string MsjBienvenidaGral { get; set; }
        public Categoria[] Categorias { get; set; }
        public ICollection<TipoSuscripcionDto> TiposDeSuscripciones {  get;set; }
        public bool esSeguido { get; set; }
        public int Categoria1Id { get; set; }
        public int Categoria2Id { get; set; }
        public Categoria Categoria1 { get; set; }
        public Categoria Categoria2 { get; set; }


    }


    public record PasswrodResetRequest
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
