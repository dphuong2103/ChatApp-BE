using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ChatAppBackEnd.Models.DatabaseModels
{
    public class ChatRoom
    {
        public const string CHATROOMTYPE_ONE = "ONE";
        public const string CHATROOMTYPE_MANY = "MANY";
        [MaxLength(100)]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string? Name { get; set; }
        
        public string ChatRoomType { get; set; } = CHATROOMTYPE_ONE;

        public bool IsDeleted { get; set; } = false;

        [MaxLength(100)]
        public string? CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual User? Creator { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedTime { get; set; }
    }
    public static class ChatRoomType {
        public const string ONE = "ONE";
        public const string MANY = "MANY";

    }
}
