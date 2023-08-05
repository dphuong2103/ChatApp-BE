using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;

namespace ChatAppBackEnd.Service.UserChatRoomService
{
    public interface IUserChatRoomService
    {
        Task<UserChatRoom> AddUserChatRoom(UserChatRoom request);
        Task<List<UserChatRoom>?> GetUserChatRoomsByUserId(string UserId);

        Task<UserChatRoom?> UpdateUserChatRoomLastMessageRead(string userChatRoomId, UpdateLastMessageRead request);

        Task<UserChatRoom?> SetMuted(string userChatRoomId, SetMutedDTO request);

        Task AddMembersToChatGroup(ChatRoomIdAndUserIds request);

        Task<UserChatRoom?> RemoveMemberFromGroupChat(UserIdAndChatRoomId request);
    }
}
