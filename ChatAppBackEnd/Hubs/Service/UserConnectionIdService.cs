using ChatAppBackEnd.Hubs.Models;
using ChatAppBackEnd.Models.DatabaseModels;

namespace ChatAppBackEnd.Hubs.Service
{
    public class UserConnectionIdService : IUserConnectionIdService
    {
        public List<UserConnectionId> _userConnectionIds { get; private set; } = new List<UserConnectionId>();
        public void Add(string userId, string connectionId)
        {
            _userConnectionIds.Add(new UserConnectionId { ConnectionId = connectionId, UserId = userId });
        }

        public void Remove(string connectionId)
        {
            try
            {
                _userConnectionIds.RemoveAll(x => x.ConnectionId == connectionId);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public List<string>? GetUsersConnectionIds(List<User> users)
        {
            var userIds = users.Select(u => u.Id);
            var connectionIds = _userConnectionIds.Where(userConnectionId => userIds.Contains(userConnectionId.UserId)).Select(uc => uc.ConnectionId).ToList();
            return connectionIds;
        }

        public List<string>? GetUsersConnectionIdsByUserIdList(List<string> userIds)
        {
            var connectionIds = _userConnectionIds.Where(userConnectionId => userIds.Contains(userConnectionId.UserId)).Select(uc => uc.ConnectionId).ToList();
            return connectionIds;
        }

        public List<string>? GetUsersConnectionIdsByUserId(string userId)
        {
            var connectionIds = _userConnectionIds.Where(userConectionId => userConectionId.UserId == userId).Select(userConnectionId => userConnectionId.ConnectionId);
            return connectionIds.ToList() ;
        }
    }
}
