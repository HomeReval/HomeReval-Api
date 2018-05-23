using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ExercisePlanning
    {
        public long ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Amount { get; set; }

        //Result must become a file / blob
        [Required]
        public bool IsComplete { get; set; }

        public long UserExercise_ID { get; set; }
        [ForeignKey("UserExercise_ID")]
        public UserExercise UserExercise { get; set; }

    }
}
