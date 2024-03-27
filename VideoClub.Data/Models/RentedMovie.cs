using System;

namespace VideoClub.Data.Models
{
    public class RentedMovie
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
