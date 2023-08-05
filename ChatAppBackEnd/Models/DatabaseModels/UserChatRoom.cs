using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackEnd.Models.DatabaseModels
{
    public class UserChatRoom
    {
        [MaxLength(100)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        [MaxLength(100)]
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [MaxLength(100)]
        [Required]
        public string ChatRoomId { get; set; }


        [ForeignKey("ChatRoomId")]
        public virtual ChatRoom ChatRoom { get; set; }

        [MaxLength(100)]
        public string? LastMessageReadId { get; set; }
        public virtual Message? LastMessageRead { get; set; }

        public bool IsMuted { get; set; } = false;
        [MaxLength(50)]
        public string MembershipStatus { get; set; } = UserChatRoomStatus.Active;
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }

    public static class UserChatRoomStatus
    {
        public const string Active = "Active";
        public const string Kicked = "Kicked";
        public const string Left = "Left";
    }
}
