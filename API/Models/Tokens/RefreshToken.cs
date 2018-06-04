using API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Tokens
{
    public class RefreshToken
    {

        public long ID { get; set; }

        public long User_ID { get; set; }

        [ForeignKey("User_ID")]
        [Required]
        public User User { get; set; }
        public string Token { get; set; }
        public bool Revoked { get; set; }
    }
}