using System.ComponentModel.DataAnnotations;


namespace Djm.OGame.Web.Api.BindingModels.Pins
{
    public class PinCreateBindingModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TargetId { get; set; }

        public int UniverseId { get; set; }
    }
}