using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoClub.Data.DataModels;
using VideoClub.Data.Models;

namespace VideoClub.Business.Services
{
    public class RentMovieService : IRentMovieService
    {
        private readonly VideoClubContext _db;

        public RentMovieService(VideoClubContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckValid(string idNumber)
        {
            var customer = await _db.Customers.Where(c => c.Idnumber == idNumber).FirstOrDefaultAsync();

            return customer != null;
        }

        public async Task<List<MovieLite>> GetMovies(string search, string idNumber)
        {
            return await _db.Movies
                .Where(s => s.DeleteDate == null
                    && s.Caption.Contains(search)
                    && s.Quantity > _db.RentedMovies
                        .Count(r => r.MovieId == s.MovieId 
                                    && r.ReturnDate == null)
                    && _db.Movies.Select(m => m.MovieId).Except(_db.RentedMovies
                            .Where(r => r.User.Idnumber == idNumber && r.ReturnDate == null)
                            .Select(r => r.MovieId))
                        .Contains(s.MovieId))
                .Select(s => new MovieLite { MovieId = s.MovieId, Caption = s.Caption, Avatar = s.Avatar })
                .ToListAsync();
        }

        public async Task<List<MovieLite>> GetMovies(string idNumber)
        {
            var rentedMoviesByUser = await _db.RentedMovies
                .Where(r => r.User.Idnumber == idNumber && r.ReturnDate == null)
                .Select(r => r.MovieId)
                .ToListAsync();

            return await _db.Movies
                .Where(s => s.DeleteDate == null)
                .Select(s => new MovieLite
                {
                    MovieId = s.MovieId,
                    Caption = s.Caption,
                    Avatar = s.Avatar,
                    AvailableForRental = !rentedMoviesByUser.Contains(s.MovieId) &&
                                              s.Quantity > _db.RentedMovies
                                                  .Count(r => r.MovieId == s.MovieId && r.ReturnDate == null)
                })
                .ToListAsync();
        }

        public async Task<List<MovieLite>> GetShowMovies(List<int> movies)
        {
            List<MovieLite> result = new List<MovieLite>();

            foreach (var movie in movies)
            {
                var movieLight = await _db.Movies
                    .Where(s => s.MovieId == movie)
                    .Select(s => new MovieLite { MovieId = s.MovieId, Caption = s.Caption, Avatar = s.Avatar })
                    .FirstOrDefaultAsync();

                result.Add(movieLight);
            }

            return result;
        }

        public async Task<List<Movie>> GetRentedForUser(string idNumber)
        {
            return await _db.Movies
                .Where(s => s.DeleteDate == null && _db.RentedMovies
                        .Where(r => r.User.Idnumber == idNumber && r.ReturnDate == null)
                        .Select(r => r.MovieId)
                        .ToList()
                        .Contains(s.MovieId))
                .ToListAsync();
        }

        public async Task<bool> RentMovies(RentRequest rentRequest)
        {
            var targetUser = await _db.Customers.Where(c => c.DeleteDate == null && c.Idnumber == rentRequest.SelectedIDnumber).FirstOrDefaultAsync();
            if (targetUser == null)
            {
                return false;
            }

            List<RentedMovie> rentedMovies = new List<RentedMovie>();

            foreach (var item in rentRequest.Movies)
            {
                var targetMovie = await _db.Movies.Where(m => m.DeleteDate == null && m.MovieId == item).FirstAsync();

                rentedMovies.Add(new RentedMovie()
                {
                    MovieId = item,
                    Movie = targetMovie,
                    UserId = targetUser.UserId,
                    User = targetUser,
                    RentDate = DateTime.UtcNow,
                    ReturnDate = null
                });
            }

            await _db.RentedMovies.AddRangeAsync(rentedMovies);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnMovies(RentRequest returnRequest)
        {
            var targetUser = await _db.Customers.Where(c => c.DeleteDate == null && c.Idnumber == returnRequest.SelectedIDnumber).FirstOrDefaultAsync();
            if (targetUser == null)
            {
                return false;
            }

            foreach (var item in returnRequest.Movies)
            {
                var target = await _db.RentedMovies
                    .Where(r => r.User.Idnumber == returnRequest.SelectedIDnumber
                        && r.MovieId == item
                        && r.ReturnDate == null)
                    .FirstOrDefaultAsync();
                if (target != null) target.ReturnDate = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return true;
        }
    }
}
