using ChatAppBackEnd.Data;
using ChatAppBackEnd.Exceptions;
using ChatAppBackEnd.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
namespace ChatAppBackEnd.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _dbContext;
        public UserService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AddUser(User request)
        {
            var user = _dbContext.Users.Find(request.Id);
            if (user is not null)
            {
                throw new UserAlreadyExistsException(user.Id);
            }
            _dbContext.Users.Add(request);
            await _dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<List<User>> SearchUsersByEmail(string filterValue)
        {
            var filterUsers = await _dbContext.Users.Where(user => user.Email.ToLower().Contains(filterValue.ToLower()) || filterValue.ToLower().Contains(user.Email.ToLower())).ToListAsync();
            return filterUsers;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }

        public async Task<User?> GetUserById(string Id)
        {
            var user = await _dbContext.Users.FindAsync(Id);
            return user;
        }

        public async Task<User?> UpdateUser(string Id, User message)
        {
            var user = await GetUserById(Id);
            if (user == null)
                return null;

            _dbContext.Entry(user).CurrentValues.SetValues(message);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> AddGoogleUser(User request)
        {
            var user = await GetUserById(request.Id);
            if (user is not null) return default;
            _dbContext.Users.Add(request);
            await _dbContext.SaveChangesAsync();
            return request;
        }
    }
}
