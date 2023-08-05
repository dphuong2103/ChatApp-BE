using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackEnd.Models.DatabaseModels
{
    public class Message
    {
        [Key]
        [MaxLength(100)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        public string MessageText { get; set; } = string.Empty;

        [MaxLength(100)]
        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ChatRoomId { get; set; }

        [ForeignKey("ChatRoomId")]
        public virtual ChatRoom? ChatRoom{get;set;}

        [MaxLength(100)]
        public string? ReplyToMessageId { get; set; }

        [ForeignKey("ReplyToMessageId")]
        public virtual Message? ReplyToMessage { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
    }
}
