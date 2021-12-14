using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Crud.DTOs;
namespace Crud.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Email is requiered")]
        [EmailAddress]
        public override string Email { get; set; }

        public string Name {get; set;}
        public bool isAdministrador { get; set; }
        public string Surname {get; set;}
        public string IdFacebook { get; set; }
        public ICollection<UserCreador> Siguiendo { get; set; }
        public ICollection<Conversacion> Conversaciones { get; set; }
        public ICollection<MediosDePagos> MediosDePago { get; set; }
        public ICollection<SuscripcionUsuario> SuscripcionUsuario { get; set; }
        public Creador Creador { get; set; }        

        public UserData GetUserData(){
            return new UserData{
                Id = Id,
                Email = Email,
                Username = UserName,
                isAdministrador = isAdministrador,
                Name = Name,
                Surname = Surname,
            };
        }
    }

    public class UserCreador
    {
        public string UserId { get; set; }
        public string CreadorId { get; set; }
        //public Creador Creador { get; set; }
        //public User User { get; set; }
    }


    public class Creador
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public virtual ICollection<UserCreador> Seguidores { get; set; } 
        public ICollection<Conversacion> ConversacionesSuscriptores { get; set; }
        public ICollection<TipoSuscripcion> TiposDeSuscripciones { get; set; }
        public ICollection<Contenido> Contenidos { get; set; }
        public Categoria Categoria1 { get; set; }
        public Categoria Categoria2 { get; set; }
        public ICollection<Finanza> Finanzas { get; set; }
        public User Usuario { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string ImagePortada { get; set; }
        public string Biografia { get; set; }
        public string VideoYoutube { get; set; }
        public string MsjBienvenidaGral { get; set; }
        public string EntidadFinanciera { get; set; }
        public string NumeroDeCuenta { get; set; }

    }
}
