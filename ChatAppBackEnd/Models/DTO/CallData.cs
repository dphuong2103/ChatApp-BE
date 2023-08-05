using ChatAppBackEnd.Models.DatabaseModels;

namespace ChatAppBackEnd.Models.DTO
{
    public class CallData
    {
        public string ChatRoomID { get; set; }
        public object SignalData { get; set; }
        public User FromUser { get; set; }
        public string FromConnectionID { get; set; }
        public string CallType { get; set; }
    }
}
