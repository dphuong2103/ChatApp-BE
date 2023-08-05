using ChatAppBackEnd.Models.DatabaseModels;

namespace ChatAppBackEnd.Hubs.Service
{
    public interface IUserConnectionIdService
    {
        void Add(string userId, string connectionId);
        void Remove(string connectionId);
        List<string>? GetUsersConnectionIds(List<User> users);
        List<string>? GetUsersConnectionIdsByUserIdList(List<string> userIds);

        List<string>? GetUsersConnectionIdsByUserId(string userId);
    }
}
