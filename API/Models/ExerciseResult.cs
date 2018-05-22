using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ExerciseResult
    {

        public long ID { get; set; }

        [Required]
        public DateTime BeginDateTime { get; set; }
        [Required]
        private int Duration { get; set; }

        [Required]
        //moet blob
        public int Score { get; set; }

        public int Exercise_ID { get; set; }

        [ForeignKey("Exercise_ID")]

        public string Name { get; set; }

        public string Description { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int ExerciseRecording_ID { get; set; }
        [ForeignKey("ExerciseRecording_ID")]
        public Exercise Exercise { get; set; }

    }
}