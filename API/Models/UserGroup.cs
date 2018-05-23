using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{

    public class UserGroup
    {

        public Type ID { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Description { get; set; }

    }

    public enum Type : int
    {
        User = 2,
        Manager = 1,
        Administrator = 0
    }
}
