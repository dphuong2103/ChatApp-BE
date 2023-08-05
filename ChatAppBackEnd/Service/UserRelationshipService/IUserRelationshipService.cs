using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.Relationships;

namespace ChatAppBackEnd.Service.UserRelationshipService
{
    public interface IUserRelationshipService
    {
        Task<UserRelationship> AddUserRelationship(NewUserRelationship request);
        Task<List<UserRelationship>?> GetUserRelationshipsByUserId(string userId);

        Task<UserRelationship?> UpdateUserRelationship(string userRelationshipId, UserRelationship request);

        Task<UserRelationship?> DeleteRelationship(string relationshipId);

        Task<UserRelationship> SendFriendRequest(NewUserRelationship request);

        Task<UserRelationship?> AcceptFriendRequest(string userRelationshipId,UserRelationship request); 
    }
}
