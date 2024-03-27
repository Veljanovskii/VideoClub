using System.Threading.Tasks;
using VideoClub.Data.DataModels;
using VideoClub.Data.Models;

namespace VideoClub.Business.Services
{
    public interface IMovieService
    {
        public Task InsertMovie(Movie movie);
        public Task<MoviesTotal> GetMovies(string sort, string order, int page, int size, string search);
        public Task<Movie> GetMovie(int id);
        public Task<bool> EditMovie(Movie movie);
        public Task<bool> DeleteMovie(int id);
    }
}
