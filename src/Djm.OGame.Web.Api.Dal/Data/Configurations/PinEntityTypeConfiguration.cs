using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class PinEntityTypeConfiguration : IEntityTypeConfiguration<Pin>
    {
        private static class Indexes
        {
            public const string UniqueUniverseOwnerTarget = "UX_UniverseOwnerTarget";
        }

        public void Configure(EntityTypeBuilder<Pin> b)
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.OwnerId)
                .IsRequired();

            b.Property(p => p.TargetId)
                .IsRequired();

            b.Property(p => p.UniverseId)
                .IsRequired();

            b.HasIndex(p => new
            {
                p.UniverseId,
                p.OwnerId,
                p.TargetId
            }).IsUnique();
        }
    }
}