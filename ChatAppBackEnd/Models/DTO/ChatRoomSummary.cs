using ChatAppBackEnd.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace ChatAppBackEnd.Models.DTO
{
    public class ChatRoomSummary
    {
        public ChatRoom ChatRoom { get; set; }
        public UserChatRoom UserChatRoom { get; set; }
        public Message? LatestMessage { get; set; }
        public List<User> Users { get; set; }
        public int NumberOfUnreadMessages { get; set; }
       
    }
}
