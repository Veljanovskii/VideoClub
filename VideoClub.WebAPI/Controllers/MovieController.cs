using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoClub.Business.Services;
using VideoClub.Data.Models;

namespace VideoClub.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator, User")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public async Task<IActionResult> Get(string sort, string order, int page, int size, string search)
        {
            try
            {
                var list = await _movieService.GetMovies(sort, order, page, size, search);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<MovieController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Movie movie)
        {
            try
            {
                await _movieService.InsertMovie(movie);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var movie = await _movieService.GetMovie(id);

                if (movie != null)
                    return Ok(movie);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        // PUT api/<MovieController>/5
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Movie movie)
        {
            try
            {
                var found = await _movieService.EditMovie(movie);

                if (found)
                    return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<MovieController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var found = await _movieService.DeleteMovie(id);

                if (found)
                    return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
