using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Exercise
    {

        public long ID { get; set; }

        [Required]
        public DateTime Startdate { get; set; }
        [Required]
        private DateTime Enddate { get; set; }

        [Required]
        public string User_id { get; set; }
        [ForeignKey("User_ID")]

        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int ExerciseRecording_ID { get; set; }
        [ForeignKey("ExerciseRecording_ID")]

        public ExerciseRecording ExerciseRecording { get; set; }

    }
}