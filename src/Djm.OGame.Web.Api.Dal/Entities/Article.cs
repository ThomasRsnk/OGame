using System;

namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastEdit { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Preview { get; set; }

        public string AuthorEmail { get; set; }
        //public virtual ApplicationUser Author { get; set; }

        public int ContentId { get; set; }
        public virtual ArticleContent Content { get; set; }
    }
}