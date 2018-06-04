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
        public string Name { get; set; }

        [Required]
=======
<<<<<<< HEAD
        public DateTime Startdate { get; set; }
        [Required]
        private DateTime Enddate { get; set; }

        [Required]
=======
        [MaxLength(100)]
>>>>>>> planning++
        public string Name { get; set; }

        [Required]
<<<<<<< HEAD
        public int Amount { get; set; }

        [Required]
        public string User_ID { get; set; }

        [ForeignKey("User_ID")]
        public User User { get; set; }

        [Required]
        public int ExerciseRecording_ID { get; set; }

        [ForeignKey("ExerciseRecording_ID")]
        public ExerciseRecording ExerciseRecording { get; set; }
=======
>>>>>>> 72773a739d6d5bb56e8375d49e1defac54bc226f
        public string Description { get; set; }
        
        // Recording must be a file / blob
        [Required]
        public byte[] Recording { get; set; }
<<<<<<< HEAD
=======
>>>>>>> planning++

>>>>>>> 72773a739d6d5bb56e8375d49e1defac54bc226f
    }
}