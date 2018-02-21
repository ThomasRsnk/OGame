using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Djm.OGame.Web.Api.BindingModels.Pins
{
    public class PinCreateBindingModel : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int OwnerId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TargetId { get; set; }

        public int UniverseId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OwnerId == TargetId)
            {
                yield return new ValidationResult("Vous ne pouvez pas vous suivre vous même (Owner = Target)", new[] { "Erreur" });
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(OwnerId)}: {OwnerId}, {nameof(TargetId)}: {TargetId}, {nameof(UniverseId)}: {UniverseId}";
        }
    }

}