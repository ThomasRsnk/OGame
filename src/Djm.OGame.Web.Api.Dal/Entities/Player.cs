namespace Djm.OGame.Web.Api.Dal.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public int UniverseId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ProfilePicturePath { get; set; }
        public string EmailAddress { get; set; }
        public bool AllowNotifications { get; set; }
        public string Role { get; set; }
        public byte[] Salt { get; set; }
    }
}