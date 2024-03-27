using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VideoClub.Data.DataModels;
using VideoClub.Data.Models;

namespace VideoClub.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly VideoClubContext _db;
        private readonly UserManager<Employee> _userManager;

        public EmployeeService(VideoClubContext db, UserManager<Employee> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task InsertEmployee(EmployeeDto employee)
        {
            var password = new PasswordHasher<EmployeeDto>();
            var hashed = password.HashPassword(employee, employee.Password);

            Employee newEmployee = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Active = true,
                UserName = employee.Email,
                NormalizedUserName = employee.Email.ToUpper(),
                Email = employee.Email,
                NormalizedEmail = employee.Email,
                EmailConfirmed = true,
                PhoneNumber = employee.PhoneNumber,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hashed
            };


            await _db.Employees.AddAsync(newEmployee);
            await _db.SaveChangesAsync();

            Employee e = await _userManager.FindByEmailAsync(newEmployee.Email);
            await _userManager.AddToRoleAsync(e, employee.Role);
        }

        public async Task<EmployeesTotal> GetEmployees(string sort, string order, int page, int size, string search)
        {
            IQueryable<Employee> employeesQuery = _db.Employees.OrderBy(s => s.Id);

            switch (sort)
            {
                case "FirstName":
                    employeesQuery = order == "desc" ? _db.Employees.OrderByDescending(s => s.FirstName) : _db.Employees.OrderBy(s => s.FirstName);
                    break;
                case "LastName":
                    employeesQuery = order == "desc" ? _db.Employees.OrderByDescending(s => s.LastName) : _db.Employees.OrderBy(s => s.LastName);
                    break;
                case "Email":
                    employeesQuery = order == "desc" ? _db.Employees.OrderByDescending(s => s.Email) : _db.Employees.OrderBy(s => s.Email);
                    break;
                case "PhoneNumber":
                    employeesQuery = order == "desc" ? _db.Employees.OrderByDescending(s => s.PhoneNumber) : _db.Employees.OrderBy(s => s.PhoneNumber);
                    break;
            }

            List<Employee> employees;
            int total;

            if (search != null && search.Length > 2)
            {
                employees = await employeesQuery
                    .Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search))
                    .Skip(page * size)
                    .Take(size)
                    .ToListAsync();
                total = await employeesQuery
                    .Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search))
                    .CountAsync();
            }
            else
            {
                employees = await employeesQuery
                    .Skip(page * size)
                    .Take(size)
                    .ToListAsync();
                total = await employeesQuery.CountAsync();
            }

            List<EmployeeDto> employeesDto = new List<EmployeeDto>();

            foreach(var employee in employees)
            {
                var roles = await _userManager.GetRolesAsync(employee);

                employeesDto.Add(new EmployeeDto()
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    Active = employee.Active,
                    Role = roles[0]
                });
            }

            EmployeesTotal employeesTotal = new EmployeesTotal
            {
                Employees = employeesDto,
                TotalEmployees = total
            };

            return employeesTotal;
        }

        public async Task<EmployeeDto> GetEmployee(string id)
        {
            var targetEmployee = await _db.Employees.Where(s => s.Id == id).FirstAsync();
            EmployeeDto employeeDto = new EmployeeDto()
            {
                Id = targetEmployee.Id,
                FirstName = targetEmployee.FirstName,
                LastName = targetEmployee.LastName,
                Email = targetEmployee.Email,
                PhoneNumber = targetEmployee.PhoneNumber,
                Active = targetEmployee.Active
            };
            var roles = await _userManager.GetRolesAsync(targetEmployee);
            employeeDto.Role = roles[0];

            return employeeDto;
        }

        public async Task<bool> EditEmployee(EmployeeDto employee)
        {
            var targetEmployee = await _db.Employees.Where(s => s.Email == employee.Email).FirstAsync();

            if (targetEmployee != null)
            {
                if (employee.Password != null)
                {
                    var password = new PasswordHasher<EmployeeDto>();
                    var hashed = password.HashPassword(employee, employee.Password);
                    targetEmployee.PasswordHash = hashed;
                }

                targetEmployee.FirstName = employee.FirstName;
                targetEmployee.LastName = employee.LastName;
                targetEmployee.Email = employee.Email;
                targetEmployee.PhoneNumber = employee.PhoneNumber;

                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ToggleEmployee(string id)
        {
            var targetEmployee = await _db.Employees.FindAsync(id);

            if (targetEmployee != null)
            {
                if (targetEmployee.Active == true)
                    targetEmployee.Active = false;
                else
                    targetEmployee.Active = true;

                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
