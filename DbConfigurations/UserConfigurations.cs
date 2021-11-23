using Crud.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Crud.DbConfigurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasMany(x => x.Siguiendo);
        }
    }

    public class CreadorConfiguration : IEntityTypeConfiguration<Creador>
    {
        public void Configure(EntityTypeBuilder<Creador> builder)
        {
            builder.HasOne(a => a.Usuario).WithOne(b => b.Creador).HasForeignKey<Creador>(b => b.UserId);
        }
    }


    public class UserCreadorConfigurations : IEntityTypeConfiguration<UserCreador>
    {
        public void Configure(EntityTypeBuilder<UserCreador> builder)
        {
            //builder.HasOne<User>(x => x.User);
            //builder.HasOne<Creador>(x => x.Creador);
            builder.HasKey(x => new { x.UserId, x.CreadorId });
        }
    }
}
