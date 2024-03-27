using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoClub.Data.DataModels;
using VideoClub.Data.Helpers;
using VideoClub.Data.Models;

namespace VideoClub.Business.Services
{
    public class UserService : IUserService
    {
        private readonly VideoClubContext _db;
        private readonly UserMapper _mapper;

        public UserService(VideoClubContext db)
        {
            _db = db;
            _mapper = new UserMapper(_db);
        }

        public async Task InsertUser(UserDto userDto)
        {
            User user = new User();
            _mapper.MapDtoToUser(user, userDto);

            await _db.Customers.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task<UsersTotal> GetUsers(string sort, string order, int page, int size, string search)
        {
            IQueryable<User> usersQuery = _db.Customers.OrderBy(s => s.UserId);

            switch (sort)
            {
                case "FirstName":
                    usersQuery = order == "desc" ? _db.Customers.OrderByDescending(s => s.FirstName) : _db.Customers.OrderBy(s => s.FirstName);
                    break;
                case "LastName":
                    usersQuery = order == "desc" ? _db.Customers.OrderByDescending(s => s.LastName) : _db.Customers.OrderBy(s => s.LastName);
                    break;
                case "Address":
                    usersQuery = order == "desc" ? _db.Customers.OrderByDescending(s => s.Address) : _db.Customers.OrderBy(s => s.Address);
                    break;
                case "Idnumber":
                    usersQuery = order == "desc" ? _db.Customers.OrderByDescending(s => s.Idnumber) : _db.Customers.OrderBy(s => s.Idnumber);
                    break;
                case "MaritalStatus":
                    usersQuery = order == "desc" ? _db.Customers.OrderByDescending(s => s.MaritalStatus) : _db.Customers.OrderBy(s => s.MaritalStatus);
                    break;
                case "InsertDate":
                    usersQuery = order == "desc" ? _db.Customers.OrderByDescending(s => s.InsertDate) : _db.Customers.OrderBy(s => s.InsertDate);
                    break;
            }

            List<User> users;
            int total;

            if (search != null && search.Length > 2)
            {
                users = await usersQuery
                    .Where(s => s.DeleteDate == null)
                    .Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search))
                    .Skip(page * size)
                    .Take(size)
                    .ToListAsync();
                total = await usersQuery
                    .Where(s => s.DeleteDate == null)
                    .Where(s => s.FirstName.Contains(search) || s.LastName.Contains(search))
                    .CountAsync();
            }
            else
            {
                users = await usersQuery.Where(s => s.DeleteDate == null)
                    .Skip(page * size)
                    .Take(size)
                    .ToListAsync();
                total = await usersQuery.Where(s => s.DeleteDate == null).CountAsync();
            }

            List<UserDto> dtoUsers = new List<UserDto>();

            foreach (var user in users)
            {
                dtoUsers.Add(_mapper.MapUserToDto(user));
            }

            UsersTotal usersTotal = new UsersTotal
            {
                Users = dtoUsers,
                TotalUsers = total
            };

            return usersTotal;
        }

        public async Task<UserDto> GetUser(int id)
        {
            var user = await _db.Customers.Where(s => s.DeleteDate == null && s.UserId == id).FirstAsync();
            return _mapper.MapUserToDto(user);
        }

        public async Task<bool> EditUser(UserDto user)
        {
            var targetUser = await _db.Customers.Where(s => s.UserId == user.UserId).SingleOrDefaultAsync();

            if (targetUser != null)
            {
                _mapper.MapDtoToUser(targetUser, user);

                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var targetUser = await _db.Customers.FindAsync(id);

            if (targetUser != null)
            {
                targetUser.DeleteDate = DateTime.Now;

                await _db.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
