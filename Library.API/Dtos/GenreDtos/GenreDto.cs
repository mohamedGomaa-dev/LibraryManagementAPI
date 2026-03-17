using System.ComponentModel.DataAnnotations;

namespace Library.API.Dtos.GenreDtos
{
    public class GenreDto
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
