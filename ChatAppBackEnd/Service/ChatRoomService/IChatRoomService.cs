using ChatAppBackEnd.Models.ChatRooms;
using ChatAppBackEnd.Models.DatabaseModels;
using ChatAppBackEnd.Models.DTO;
using ChatAppBackEnd.Models.UserChatRooms;

namespace ChatAppBackEnd.Service.ChatRoomService
{
    public interface IChatRoomService
    {
        Task<ChatRoom?> GetChatRoomById(string Id);
        Task<ChatRoomSummary> AddChatRoom(NewChatRoomAndUserList newChatRoomAndUserList);
        Task<ChatRoom?> UpdateChatRoom(string Id,ChatRoom request);
        Task<List<ChatRoom>> GetAllChatRooms();
        Task<List<ChatRoomSummary>> GetChatRoomSummariesByUserId(string userId);
        Task<ChatRoomSummary> AddChatRoomGroup(NewChatRoomAndUserList request);
        Task<ChatRoom> UpdateChatRoomName(string chatRoomId,ChatRoomIdAndName request);
    }
}
