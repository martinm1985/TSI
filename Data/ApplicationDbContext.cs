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

        }

        public DbSet<MediosDePagos> MediosDePagos { get; set; }

        public DbSet<PagoPayPal> PagosPayPal { get; set; }

        public DbSet<SuscripcionPayPal> SuscripcionesPayPal { get; set; }

        public DbSet<Pago> Pagos {get; set; }

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
