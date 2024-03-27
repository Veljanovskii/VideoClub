using System.Threading.Tasks;
using VideoClub.Data.DataModels;

namespace VideoClub.Business.Services
{
    public interface IEmployeeService
    {
        public Task InsertEmployee(EmployeeDto employee);
        public Task<EmployeesTotal> GetEmployees(string sort, string order, int page, int size, string search);
        public Task<EmployeeDto> GetEmployee(string id);
        public Task<bool> EditEmployee(EmployeeDto employee);
        public Task<bool> ToggleEmployee(string id);
    }
}
