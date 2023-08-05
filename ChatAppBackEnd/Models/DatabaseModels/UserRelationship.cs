using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackEnd.Models.DatabaseModels
{
    public class UserRelationship
    {
        public static string RelationshipStatus_Accepted = "E_ACCEPTED";
        public static string RelationshipStatus_Declined = "E_DECLINED";
        public static string RelationshipStatus_Pending = "E_PENDING";
        public static string RelationshipStatus_Delete = "E_DELETED";
        public static string RelationshipType_Friend = "E_FRIEND";
        public static string RelationshipType_Block = "E_BLOCK";
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [MaxLength(100)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string InitiatorUserId { get; set; }
        [ForeignKey("InitiatorUserId")]
        public virtual User? InitiatorUser { get; set; }

        [MaxLength(100)]
        public string TargetUserId { get; set; }
        [ForeignKey("TargetUserId")]
        public virtual User? TargetUser { get; set; }
        [MaxLength(50)]
        public string RelationshipType { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = RelationshipStatus_Pending;
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
