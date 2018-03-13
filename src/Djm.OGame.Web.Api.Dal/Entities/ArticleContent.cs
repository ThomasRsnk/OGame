namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class ArticleContent
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string HtmlContent { get; set; }
    }
}