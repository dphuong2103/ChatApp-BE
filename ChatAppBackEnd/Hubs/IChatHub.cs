using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;

namespace ChatAppBackEnd.Hubs
{
    public interface IChatHub
    {
        Task ReceivedChatroom(ChatRoomSummary chatRoomSummary);
        Task ReceivedMessage(Message message);

        Task UpdateConnectionId(string connectionId);

        Task ReceiveCall(CallData callData);

        Task CallAccepted(object callData);

        Task LeavedCall(string chatRoomId);

        Task CallDeclined(string fromConnectionId);

        Task ReceivedRelationship(UserRelationship userRelationship, string type);

        Task RemoveUserFromGroupChat(UserIdAndChatRoomId request);
        Task AddMembersToChatRoom(ChatRoomIdAndUsers request);

        Task UpdateChatRoomName(ChatRoomIdAndName request);
    }
}
