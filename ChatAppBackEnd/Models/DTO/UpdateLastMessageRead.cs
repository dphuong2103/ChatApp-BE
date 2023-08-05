using System.ComponentModel.DataAnnotations;

namespace ChatAppBackEnd.Models.DTO
{
    public class UpdateLastMessageRead
    {
        [MaxLength(100)]
        [Required]
        public string Id { get; set; }
        public string LastMessageReadId { get; set; }
    }
}
