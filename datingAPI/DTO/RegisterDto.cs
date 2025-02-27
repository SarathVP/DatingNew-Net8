

using System.ComponentModel.DataAnnotations;

namespace datingAPI.DTO
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string passWord { get; set; }
    }
}