using ChatAppBackEnd.Models.DatabaseModels;

namespace ChatAppBackEnd.Models.UserChatRooms
{
    public class ChatRoomIdAndUsers
    {
        public string ChatRoomId { get; set; }
        public List<User> Users { get; set; }
    }
}
