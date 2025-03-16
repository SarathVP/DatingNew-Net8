

using System.ComponentModel.DataAnnotations;

namespace datingAPI.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 6)]
        public string passWord { get; set; } = string.Empty;
    }
}