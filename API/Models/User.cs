using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {

        public long ID { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        // Because of security reasons, the password is never returned in the Controller
        [JsonIgnore]
        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public char Gender { get; set; }

        public Type UserGroup_ID { get; set; }

        [ForeignKey("UserGroup_ID")]
        [Required]
        public UserGroup UserGroup { get; set; }

    
    }
}
