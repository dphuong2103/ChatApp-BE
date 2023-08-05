using ChatAppBackEnd.Models.DatabaseModels;

namespace ChatAppBackEnd.Service.UserService
{
    public interface IUserService
    {
        Task<User?> GetUserById(string Id);
        Task<List<User>> SearchUsersByEmail(string filterValue);

        Task<List<User>> GetAllUsers();

        Task<User> AddUser(User user);

        Task<User?> UpdateUser(string Id,User user);

    }
}
