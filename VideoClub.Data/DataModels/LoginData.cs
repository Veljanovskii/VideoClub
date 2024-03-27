using System.ComponentModel.DataAnnotations;

namespace VideoClub.Data.DataModels
{
    public class LoginData
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
