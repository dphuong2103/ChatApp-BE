using ChatAppBackEnd.Models.DatabaseModels;

namespace ChatAppBackEnd.Models.DTO
{
    public class NewChatRoomAndUserList
    {
        public NewChatRoom NewChatRoom { get; set; }
        public List<string> UserIds { get; set; }
    }
}
