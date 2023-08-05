namespace ChatAppBackEnd.Models.DTO
{
    public class NewChatRoom
    {
        public const string CHATROOMTYPE_ONE = "ONE";
        public const string CHATROOMTYPE_MANY = "MANY";
        public string? Name { get; set; }
        public string ChatRoomType { get; set; } = CHATROOMTYPE_ONE;
        public string CreatorId { get; set; }
    }
}
