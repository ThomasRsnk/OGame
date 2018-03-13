using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class ArticleContentEntityTypeConfiguration : IEntityTypeConfiguration<ArticleContent>
    {
        public void Configure(EntityTypeBuilder<ArticleContent> b)
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.AuthorId)
                .IsRequired();

            b.Property(p => p.HtmlContent)
                .IsRequired();
        }
    }
}