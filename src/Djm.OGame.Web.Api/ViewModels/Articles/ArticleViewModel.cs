using System;


namespace Djm.OGame.Web.Api.ViewModels.Articles
{
    public class ArticleViewModel 
    {
        public int Id { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishDate { get; set; }
        public string FormatedPublishDate { get; set; }
        public DateTime LastEdit { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Preview { get; set; }
    }
}