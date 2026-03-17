using System.ComponentModel.DataAnnotations;

namespace Library.API.Dtos.UserDtos
{
    public class UserCreateDto
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

        [MinLength(5)]
        [MaxLength(50)]
        public string LibraryCardNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }
}
