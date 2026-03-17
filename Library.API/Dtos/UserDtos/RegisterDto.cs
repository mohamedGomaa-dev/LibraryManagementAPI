using System.ComponentModel.DataAnnotations;

namespace Library.API.Dtos.UserDtos
{
    public class RegisterDto
    {
        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        [MinLength(5)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [MaxLength(30)]
        [MinLength(5)]
        public string? Phone { get; set; }

        

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
