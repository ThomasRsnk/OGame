namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class ArticleContent
    {
        public int Id { get; set; }

        public string HtmlContent { get; set; }

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}