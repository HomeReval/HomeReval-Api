using System;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{

    public class ExerciseRecording
    {

        public int ID { get; set; }

        [Required]
        // moet blob zijn
        public string Recording { get; set; }

    }
}
