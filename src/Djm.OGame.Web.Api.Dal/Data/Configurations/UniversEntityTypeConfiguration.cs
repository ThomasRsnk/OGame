using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class UniversEntityTypeConfiguration : IEntityTypeConfiguration<Univers>
    {
     
        public void Configure(EntityTypeBuilder<Univers> b)
        {
            b.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();

            b.Property(p => p.Name)
                .IsRequired();
        }
    }
}