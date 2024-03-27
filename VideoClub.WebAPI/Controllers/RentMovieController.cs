using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoClub.Business.Services;
using VideoClub.Data.DataModels;

namespace VideoClub.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentMovieController : ControllerBase
    {

        private readonly IRentMovieService _rentMovieService;

        public RentMovieController(IRentMovieService rentMovieService)
        {
            _rentMovieService = rentMovieService;
        }

        [HttpGet("GetMovies")]
        public async Task<IActionResult> GetMovies(string idNumber)
        {
            try
            {
                var list = await _rentMovieService.GetMovies(idNumber);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("Search")]
        public async Task<IActionResult> Search(string search, string idNumber)
        {
            try
            {
                var list = await _rentMovieService.GetMovies(search, idNumber);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Show")]
        public async Task<IActionResult> Show([FromQuery] List<int> movies)
        {
            try
            {
                var list = await _rentMovieService.GetShowMovies(movies);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetRented")]
        public async Task<IActionResult> GetRentedMovies(string idNumber)
        {
            try
            {
                var list = await _rentMovieService.GetRentedForUser(idNumber);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CheckValid")]
        public async Task<IActionResult> CheckValid(string idNumber)
        {
            try
            {
                var found = await _rentMovieService.CheckValid(idNumber);

                return Ok(found);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RentMovies([FromBody] RentRequest rentRequest)
        {
            try
            {
                var success = await _rentMovieService.RentMovies(rentRequest);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Return")]
        public async Task<IActionResult> ReturnMovies([FromBody] RentRequest rentRequest)
        {
            try
            {
                var success = await _rentMovieService.ReturnMovies(rentRequest);

                return Ok(success);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
