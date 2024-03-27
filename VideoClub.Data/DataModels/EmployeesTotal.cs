using System.Collections.Generic;

namespace VideoClub.Data.DataModels
{
    public class EmployeesTotal
    {
        public List<EmployeeDto> Employees { get; set; }
        public int TotalEmployees { get; set; }
    }
}
