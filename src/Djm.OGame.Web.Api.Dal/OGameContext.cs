using Djm.OGame.Web.Api.Dal.Data.Configurations;
using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Djm.OGame.Web.Api.Dal
{
    public class OGameContext : DbContext
    {
        public OGameContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Pin> Pins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PinEntityTypeConfiguration());
        }
    }
}
