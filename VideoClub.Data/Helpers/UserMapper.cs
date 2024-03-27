using System.Linq;
using System.Text;
using VideoClub.Data.DataModels;
using VideoClub.Data.Models;

namespace VideoClub.Data.Helpers
{
    public class UserMapper
    {
        private readonly VideoClubContext _db;

        public UserMapper(VideoClubContext db)
        {
            _db = db;
        }

        public UserDto MapUserToDto(User user)
        {
            UserDto userDto = new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Idnumber = user.Idnumber,
                MaritalStatus = _db.MaritalStatuses.Where(s => s.MaritalStatusId == user.MaritalStatusId).Select(s => s.Caption).First(),
                InsertDate = user.InsertDate,
                DeleteDate = user.DeleteDate
            };

            if (user.ProfilePicture != null)
                userDto.ProfilePicture = Encoding.UTF8.GetString(user.ProfilePicture);

            return userDto;
        }

        public User MapDtoToUser(User user, UserDto userDto)
        {
            user.UserId = userDto.UserId;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Address = userDto.Address;
            user.Idnumber = userDto.Idnumber;
            user.MaritalStatusId = _db.MaritalStatuses.Where(s => s.Caption == userDto.MaritalStatus).Select(s => s.MaritalStatusId).First();
            user.InsertDate = userDto.InsertDate;
            user.DeleteDate = userDto.DeleteDate;

            if (!string.IsNullOrEmpty(userDto.ProfilePicture)) 
                user.ProfilePicture = Encoding.UTF8.GetBytes(userDto.ProfilePicture);

            return user;
        }
    }
}
