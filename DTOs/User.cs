using Crud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string Id {get; set;}
        public string Name { get; set; }
        public string Email { get; set; }
        public bool isAdministrador { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }

        public CreadorData Creador {get; set;} 
    }

    public record CreadorRegister
    {
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string ImagePortada { get; set; }
        public string Biografia { get; set; }
        public string VideoYoutube { get; set; }
        public string MsjBienvenidaGral { get; set; }
        public int Categoria1Id { get; set; }
        public int Categoria2Id { get; set; }
        public string UserId { get; set; }

    }
    public record CreadorData
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string ImagePortada { get; set; }
        public string Biografia { get; set; }
        public string VideoYoutube { get; set; }
        public string MsjBienvenidaGral { get; set; }
        public string[] Categorias { get; set; }

    }

}
