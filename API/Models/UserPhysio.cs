using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserPhysio
    {

        public long UserID { get; set; }

        public long PhysioID { get; set; }

        [ForeignKey("UserID")]
        [Required]
        public User User { get; set; }

        [ForeignKey("PhysioID")]
        [Required]
        public User Physio { get; set; }

    }
}
