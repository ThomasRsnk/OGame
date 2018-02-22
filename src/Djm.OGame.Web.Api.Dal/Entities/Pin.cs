namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class Pin 
    {
        public int Id { get; set; }
        
        public int OwnerId { get; set; }
        public int TargetId { get; set; }
        public int UniverseId { get; set; }
    }
}
