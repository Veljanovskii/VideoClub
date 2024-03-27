using System.Collections.Generic;
using System.Threading.Tasks;
using VideoClub.Data.DataModels;
using VideoClub.Data.Models;

namespace VideoClub.Business.Services
{
    public interface IRentMovieService
    {
        public Task<List<MovieLite>> GetMovies(string search, string idNumber);
        public Task<List<MovieLite>> GetMovies(string idNumber);
        public Task<List<MovieLite>> GetShowMovies(List<int> movies);
        public Task<bool> RentMovies(RentRequest rentRequest);
        public Task<bool> ReturnMovies(RentRequest returnRequest);
        public Task<bool> CheckValid(string idNumber);
        public Task<List<Movie>> GetRentedForUser(string idNumber);
    }
}
