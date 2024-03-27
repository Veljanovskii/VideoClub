using System;

namespace VideoClub.Data.DataModels
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Idnumber { get; set; }
        public string MaritalStatus { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
