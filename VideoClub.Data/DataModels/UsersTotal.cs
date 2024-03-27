using System.Collections.Generic;

namespace VideoClub.Data.DataModels
{
    public class UsersTotal
    {
        public List<UserDto> Users { get; set; }
        public int TotalUsers { get; set; }
    }
}
