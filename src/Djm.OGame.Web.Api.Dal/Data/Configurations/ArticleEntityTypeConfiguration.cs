using Djm.OGame.Web.Api.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Djm.OGame.Web.Api.Dal.Data.Configurations
{
    public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> b)
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.Title)
                .IsRequired();

            b.Property(p => p.HtmlContentId)
                .IsRequired();
        }
    }
}