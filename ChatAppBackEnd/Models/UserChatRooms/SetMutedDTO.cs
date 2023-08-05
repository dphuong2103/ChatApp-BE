using System.ComponentModel.DataAnnotations;

namespace ChatAppBackEnd.Models.UserChatRooms
{
    public class SetMutedDTO
    {
        [MaxLength(100)]
        public string Id { get; set; }
        public bool IsMuted { get; set; }
    }
}
