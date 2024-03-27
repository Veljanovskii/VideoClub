using System;
using System.Collections.Generic;

namespace VideoClub.Data.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Caption { get; set; }
        public int ReleaseYear { get; set; }
        public int MovieLength { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Avatar { get; set; }
        public int Quantity { get; set; }

        public ICollection<RentedMovie> RentedMovies { get; set; }
    }
}