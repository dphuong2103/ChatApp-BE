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
        public virtual ChatRoom? ChatRoom { get; set; }

        [MaxLength(100)]
        public string? ReplyToMessageId { get; set; }

        [ForeignKey("ReplyToMessageId")]
        public virtual Message? ReplyToMessage { get; set; }

        [MaxLength(50)]
        public string Type { get; set; } = MessageType.PlainText;
        public string? FileUrls { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool IsDeleted { get; set; } = false;
        [MaxLength(50)]
        public string? FileStatus { get; set; }

        public string? FileName { get; set; }
    }

    public static class MessageType
    {
        [MaxLength(50)]
        public static string PlainText = "PlainText";
        [MaxLength(50)]
        public static string Files = "Files";
        [MaxLength(50)]
        public static string AudioRecord = "AudioRecord";
    }

    public static class FileStatus
    {
        [MaxLength(50)]
        public static string InProgress = "InProgress";
        [MaxLength(50)]
        public static string Cancelled = "Cancelled";
        [MaxLength(50)]
        public static string Done = "Done";
        [MaxLength(50)]
        public static string Error = "Error";
    }
}
