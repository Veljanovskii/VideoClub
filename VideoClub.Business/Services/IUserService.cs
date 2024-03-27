using System.Threading.Tasks;
using VideoClub.Data.DataModels;

namespace VideoClub.Business.Services
{
    public interface IUserService
    {
        public Task InsertUser(UserDto user);
        public Task<UsersTotal> GetUsers(string sort, string order, int page, int size, string search);
        public Task<UserDto> GetUser(int id);
        public Task<bool> EditUser(UserDto user);
        public Task<bool> DeleteUser(int id);
    }
}
