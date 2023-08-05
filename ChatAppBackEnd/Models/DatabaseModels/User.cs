using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppBackEnd.Models.DatabaseModels
{
    public class User
    {
        [MaxLength(100)]
        [Key]
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Gender { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
    }
}
