using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class MoveRequest
    {
        [Required]
        public string Uci { get; set; } = string.Empty;
        [Required]
        public string GameId { get; set; } = string.Empty;
    }
}
