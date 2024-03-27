using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoClub.Data.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Idnumber { get; set; }
        public int MaritalStatusId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        [Column("Profile picture")]
        public byte[] ProfilePicture { get; set; }

        public virtual MaritalStatus MaritalStatus { get; set; }
        public ICollection<RentedMovie> RentedMovies { get; set; }
    }
}