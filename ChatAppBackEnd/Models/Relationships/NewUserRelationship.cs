using ChatAppBackEnd.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations;

namespace ChatAppBackEnd.Models.Relationships
{
    public class NewUserRelationship
    {
        [MaxLength(100)]
        public string InitiatorUserId { get; set; }
        [MaxLength(100)]
        public string TargetUserId { get; set; }
        [MaxLength(50)]
        public string RelationshipType { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = UserRelationship.RelationshipStatus_Pending;
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }
}
