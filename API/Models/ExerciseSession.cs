using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ExerciseSession
    {
        public long ID { get; set; }
        public DateTime Date { get; set; }
        public bool IsComplete { get; set; }
        public ExerciseResult ExerciseResult { get; set; }
        public long ExercisePlanning_ID { get; set; }
        [ForeignKey("ExercisePlanning_ID")]
        public ExercisePlanning ExercisePlanning { get; set; }
    }
}
