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
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Administrator")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<IActionResult> Get(string sort, string order, int page, int size, string search)
        {
            try
            {
                var list = await _employeeService.GetEmployees(sort, order, page, size, search);

                if (list != null)
                    return Ok(list);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] EmployeeDto employee)
        {
            try
            {
                await _employeeService.InsertEmployee(employee);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var employee = await _employeeService.GetEmployee(id);

                if (employee != null)
                    return Ok(employee);
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EmployeeDto employee)
        {
            try
            {
                var found = await _employeeService.EditEmployee(employee);

                if (found)
                    return Ok();
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var found = await _employeeService.ToggleEmployee(id);

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
