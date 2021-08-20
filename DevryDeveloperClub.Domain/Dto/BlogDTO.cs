using System.ComponentModel.DataAnnotations;

namespace DevryDeveloperClub.Domain.Dto
{
    public class BlogDTO
    {
        [Required]
        public string Category { get; set; }
    }
}