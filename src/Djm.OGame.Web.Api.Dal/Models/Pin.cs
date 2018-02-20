using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Djm.OGame.Web.Api.Dal.Models
{
    public class Pin 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int TargetId { get; set; }

        [Required]
        public int UniverseId { get; set; }


        public Pin()
        {

        }


    }
}
