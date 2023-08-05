using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackEnd.Models.DatabaseModels
{
    public class UserChatRoomSettings
    {
        [Key]
        [ForeignKey("UserChatRoomId")]

        public string UserChatRoomId { get; set; }
        public UserChatRoom UserChatRoom { get; set; }
        public bool IsMuted { get; set; }
    }
}