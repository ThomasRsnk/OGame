using Djm.OGame.Web.Api.Dal.Data.Configurations;
using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Djm.OGame.Web.Api.Dal
{
    public class OGameContext : IdentityDbContext<ApplicationUser>
    {
        public OGameContext(DbContextOptions<OGameContext> options) : base(options)
        {
        }

        public DbSet<Pin> Pins { get; set; }
        public DbSet<Univers> Univers { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleContent> ArticlesContents { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PinEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UniversEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleContentEntityTypeConfiguration());
        }

       
    }
}
