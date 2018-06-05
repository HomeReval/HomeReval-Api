using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Form
{
    public class ClientExerciseResult
    {

        public long ID { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public string Result { get; set; }
        [Required]
        public long ExercisePlanning_ID { get; set; }


    }

}
