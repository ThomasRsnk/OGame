using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
    {

        public void Configure(EntityTypeBuilder<Player> b)
        {
            b.HasKey(p => new {p.Id, p.UniverseId});

            b.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(p => p.UniverseId)
                .IsRequired()
                .ValueGeneratedNever();
            
            b.HasIndex(p => new
            {
                p.UniverseId,
                p.Id,
                p.Login
            }).IsUnique();
        }
    }
}