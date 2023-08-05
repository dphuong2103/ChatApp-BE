using System.ComponentModel.DataAnnotations;

namespace ChatAppBackEnd.Models.DTO
{
    public class NewMessage
    {
        public string MessageText { get; set; } = string.Empty;

        [StringLength(100)]
        public string SenderId { get; set; }

        [Required]
        [StringLength(100)]
        public string ChatRoomId { get; set; }

        [MaxLength(100)]
        public string? ReplyToMessageId { get; set; }
    }
}
