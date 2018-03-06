namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public int UniverseId { get; set; }
        public string ProfilePicturePath { get; set; }
        public string EmailAddress { get; set; }
        public bool AllowNotifications { get; set; }
    }
}