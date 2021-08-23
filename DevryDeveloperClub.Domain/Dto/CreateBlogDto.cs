using System.ComponentModel.DataAnnotations;

namespace DevryDeveloperClub.Domain.Dto
{
    public class CreateBlogDto
    {
        [Required]
        public string Category { get; set; }
    }
}