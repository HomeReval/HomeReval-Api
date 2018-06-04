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
<<<<<<< HEAD
        [MaxLength(100)]
=======
        public DateTime Startdate { get; set; }
        [Required]
        private DateTime Enddate { get; set; }

        [Required]
>>>>>>> 214764f7575e4d1de98eecdc39556a893d779a97
        public string Name { get; set; }

        [Required]
<<<<<<< HEAD
        public string Description { get; set; }
        
        // Recording must be a file / blob
        [Required]
        public byte[] Recording { get; set; }
=======
        public int Amount { get; set; }

        [Required]
        public string User_ID { get; set; }

        [ForeignKey("User_ID")]
        public User User { get; set; }

        [Required]
        public int ExerciseRecording_ID { get; set; }

        [ForeignKey("ExerciseRecording_ID")]
        public ExerciseRecording ExerciseRecording { get; set; }
>>>>>>> 214764f7575e4d1de98eecdc39556a893d779a97

    }
}