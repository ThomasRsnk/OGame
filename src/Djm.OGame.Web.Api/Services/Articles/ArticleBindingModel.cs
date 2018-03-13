using System;

namespace Djm.OGame.Web.Api.Services.Articles
{
    public class ArticleBindingModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastEdit { get; set; }
        public string PathToHtml { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Preview { get; set; }
    }
}