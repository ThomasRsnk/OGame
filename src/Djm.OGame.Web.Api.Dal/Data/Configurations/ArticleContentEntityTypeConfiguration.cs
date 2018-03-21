using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class ArticleContentEntityTypeConfiguration : IEntityTypeConfiguration<ArticleContent>
    {
        public void Configure(EntityTypeBuilder<ArticleContent> b)
        {
            b.HasKey(ac => ac.Id);

            b.Property(ac => ac.HtmlContent)
                .IsRequired();

            b.HasOne(ac => ac.Article)
                .WithOne(a => a.Content)
                .HasForeignKey((Article a) => a.ContentId)
                .HasPrincipalKey((ArticleContent ac) => ac.ArticleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}