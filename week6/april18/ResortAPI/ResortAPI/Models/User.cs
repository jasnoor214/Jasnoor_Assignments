using System.ComponentModel.DataAnnotations;

namespace ResortAPI.Models
{
    public class User
    {

        [Key]
        public long? UserId { get; set; }//n

        public string Email { get; set; }

        public string Password { get; set; }

        public string? Username { get; set; }

        public string? MobileNumber { get; set; }

        public string? UserRole { get; set; }
    }
}
