using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
    {

        public void Configure(EntityTypeBuilder<Player> b)
        {
            b.HasKey(p => p.EmailAddress);

            b.Property(p => p.OGameId)
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(p => p.UniverseId)
                .IsRequired()
                .ValueGeneratedNever();
            
            b.HasIndex(p => new
            {
                p.UniverseId,
                p.OGameId,
            }).IsUnique();
        }
    }
}