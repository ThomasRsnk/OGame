using System.ComponentModel.DataAnnotations;

namespace Djm.OGame.Web.Api.ViewModels.Articles
{
    public class ArticleEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(285)]
        public string Preview { get; set; }

        [Required]
        public string HtmlContent { get; set; }
    }
}