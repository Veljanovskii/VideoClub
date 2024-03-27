using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoClub.Business.Services;
using VideoClub.Data.DataModels;

namespace VideoClub.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator, User")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IActionResult> Get(string sort, string order, int page, int size, string search)
        {
            try
            {
                var list = await _userService.GetUsers(sort, order, page, size, search);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto user)
        {
            try
            {
                await _userService.InsertUser(user);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);

                if (user != null)
                    return Ok(user);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserDto user)
        {
            try
            {
                var found = await _userService.EditUser(user);

                if (found)
                    return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var found = await _userService.DeleteUser(id);

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
