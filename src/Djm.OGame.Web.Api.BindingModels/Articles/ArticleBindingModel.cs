using System;
using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.BindingModels.Articles
{
    public class ArticleListItemBindingModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastEdit { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Preview { get; set; }
    }

    public class CreateArticleBindingModel
    {
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Preview { get; set; }
        [Required]
        public string HtmlContent { get; set; }
    }

    public class ArticleDetailsBindingModel : ArticleListItemBindingModel
    {
        public string HtmlContent { get; set; }
    }
}