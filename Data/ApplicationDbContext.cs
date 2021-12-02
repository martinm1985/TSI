using System;
using Crud.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Crud.DbConfigurations;

namespace Crud.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            //modelbuilder.ApplyConfiguration(new UserConfigurations());
            //modelbuilder.ApplyConfiguration(new CreadorConfiguration());
            modelbuilder.ApplyConfiguration(new UserCreadorConfigurations());

            modelbuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>
            {
              new IdentityRole {
                Id = "1",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
              },
            });
            var adminUser = new User
            {
                Id = "1", // primary key
                UserName = "CreadoresUY",
                NormalizedUserName = "CREADORESUY",
                NormalizedEmail = "CREADORESUYADMIN@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                Name = "Creadores",
                Surname = "UY",
                Email = "creadoresuyadmin@gmail.com",
                isAdministrador = true
            };

            PasswordHasher<User> ph = new PasswordHasher<User>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Temporal123!");
            modelbuilder.Entity<User>().HasData(adminUser);

            modelbuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1",
                UserId = "1"
            });

            TipoSuscripcion tipoSuscripcion1 = new()
            {
                Id = 1,
                Creador = null,
                Nombre = "Basico",
                Precio = 1,
                Imagen = "TODO",
                Beneficios = "Acceso a un nuevo contenido por semana;;",
                MensajeBienvenida = "",
                VideoBienvenida = false,
                MensajeriaActiva = false,
                Activo = true,
                IncluyeTipoSuscrId = null,
            };

            modelbuilder.Entity<TipoSuscripcion>().HasData(tipoSuscripcion1);

            Parametros parametro1 = new()
            {
                Nombre = "SUSCDEFECTO1",
                Valor = "1"

            };

            modelbuilder.Entity<Parametros>().HasData(parametro1);

            
            TipoSuscripcion tipoSuscripcion2 = new()
            {
                Id = 2,
                Creador = null,
                Nombre = "Estandar",
                Precio = 5,
                Imagen = "TODO",
                Beneficios = "Acceso a todo el contenido subido;;",
                MensajeBienvenida = "",
                VideoBienvenida = false,
                MensajeriaActiva = false,
                Activo = true,
                IncluyeTipoSuscrId = tipoSuscripcion1.Id,
            };

            modelbuilder.Entity<TipoSuscripcion>().HasData(tipoSuscripcion2);

            Parametros parametro2 = new()
            {
                Nombre = "SUSCDEFECTO2",
                Valor = "2"

            };

            modelbuilder.Entity<Parametros>().HasData(parametro2);

            TipoSuscripcion tipoSuscripcion3 = new()
            {
                Id = 3,
                Creador = null,
                Nombre = "Premium",
                Precio = 10,
                Imagen = "TODO",
                Beneficios = "Todo lo que incluye el estandar;;CHATEA CONMIGO;;",
                MensajeBienvenida = "",
                VideoBienvenida = false,
                MensajeriaActiva = true,
                Activo = true,
                IncluyeTipoSuscrId = tipoSuscripcion2.Id,
            };

            modelbuilder.Entity<TipoSuscripcion>().HasData(tipoSuscripcion3);

            Parametros parametro3 = new()
            {
                Nombre = "SUSCDEFECTO3",
                Valor = "3"

            };

            modelbuilder.Entity<Parametros>().HasData(parametro3);

            Parametros parametro4 = new()
            {
                Nombre = "GananciaCreador",
                Valor = "0.9"

            };


            modelbuilder.Entity<Parametros>().HasData(parametro4);

            Categoria categoria1 = new()
            {
                Id = 1,
                Nombre = "Fotografia",

            };
            modelbuilder.Entity<Categoria>().HasData(categoria1);

            Categoria categoria2 = new()
            {
                Id = 2,
                Nombre = "Videos",

            };
            modelbuilder.Entity<Categoria>().HasData(categoria2);

        }

        public DbSet<MediosDePagos> MediosDePagos { get; set; }

        public DbSet<Tarjeta> Tarjetas { get; set; }

        public DbSet<Cuenta> Cuentas { get; set; }

        public DbSet<PayPal> Paypals { get; set; }

        public DbSet<EntidadFinanciera> EntidadesFinancieras { get; set; }

        public DbSet<Pago> Pagos { get; set; }

        public DbSet<PagoPayPal> PagosPayPal { get; set; }

        public DbSet<DevolucionPayPal> DevolucionesPayPal { get; set; }

        public DbSet<Crud.Models.Mensaje> Mensaje { get; set; }

        public DbSet<Crud.Models.Conversacion> Conversacion { get; set; }

        public DbSet<Crud.Models.FinanzaPlataforma> FinanzaPlataforma { get; set; }

        public DbSet<Crud.Models.Finanza> Finanza { get; set; }

        public DbSet<Crud.Models.TipoSuscripcion> TipoSuscripcion { get; set; }

        public DbSet<Crud.Models.HistoricoSuscripcion> HistoricoSuscripcion { get; set; }

        public DbSet<Crud.Models.SuscripcionUsuario> SuscripcionUsuario { get; set; }

        public DbSet<Crud.Models.Contenido> Contenido { get; set; }

        public DbSet<Crud.Models.Audio> Audio { get; set; }

        public DbSet<Crud.Models.Video> Video { get; set; }

        public DbSet<Crud.Models.Texto> Texto { get; set; }

        public DbSet<Crud.Models.Link> Link { get; set; }

        public DbSet<Crud.Models.LiveStream> LiveStream { get; set; }

        public DbSet<Crud.Models.Imagen> Imagen { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Crud.Models.Parametros> Parametros { get; set; }

        public DbSet<Crud.Models.Categoria> Categoria { get; set; }

        public DbSet<Crud.Models.User> Usuarios { get; set; }

        public DbSet<Crud.Models.Creador> Creadores { get; set; }


    }
}