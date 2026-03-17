using System.ComponentModel.DataAnnotations;

namespace Library.API.Dtos.UserDtos
{
    public class LoginDto
    {

        [MaxLength(250)]
        [MinLength(5)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
