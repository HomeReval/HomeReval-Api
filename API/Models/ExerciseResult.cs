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
        public int Duration { get; set; }
        [Required]
        public int Score { get; set; }
        //Result must become a file / blob
        [Required]
        public byte[] Result { get; set; }
        public long ExerciseSession_ID { get; set; }
        [ForeignKey("ExerciseSession_ID")]
        public ExerciseSession ExerciseSession { get; set; }
    }
}