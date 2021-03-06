﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class UserPhysio
    {

        public long User_ID { get; set; }

        public long Physio_ID { get; set; }

        [ForeignKey("User_ID")]
        [Required]
        public User User { get; set; }

        [ForeignKey("Physio_ID")]
        [Required]
        public User Physio { get; set; }

    }
}
