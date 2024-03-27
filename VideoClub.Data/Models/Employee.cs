using Microsoft.AspNetCore.Identity;

namespace VideoClub.Data.Models
{
    public class Employee : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
    }
}