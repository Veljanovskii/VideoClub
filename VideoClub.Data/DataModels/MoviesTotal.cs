using System.Collections.Generic;
using VideoClub.Data.Models;

namespace VideoClub.Data.DataModels
{
    public class MoviesTotal
    {
        public List<Movie> Movies { get; set; }
        public int TotalMovies { get; set; }
    }
}
