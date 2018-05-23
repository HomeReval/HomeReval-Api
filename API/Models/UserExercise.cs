using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserExercise
    {

        public long ID { get; set; }

        public long User_ID { get; set; }

        public long Exercise_ID { get; set; }

        [ForeignKey("User_ID")]
        [Required]
        public User User { get; set; }

        [ForeignKey("Exercise_ID")]
        [Required]
        public Exercise Exercise { get; set; }

    }
}
