using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;

namespace ChatAppBackEnd.Service.HubService
{
    public interface IHubService
    {
        Task SendChatRoomInfo(ChatRoomSummary chatRoomSummary);
        Task SendMessage(Message message);
        Task SendRelationship(UserRelationship userRelationship, string type);
        Task RemoveUserFromGroupChat(UserIdAndChatRoomId request);

        Task AddMembersToChatGroup(ChatRoomIdAndUsers request);

        Task UpdateChatRoomName(ChatRoomIdAndName request);
    }
}
