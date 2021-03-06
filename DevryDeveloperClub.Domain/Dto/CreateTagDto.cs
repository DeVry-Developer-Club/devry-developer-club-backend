using System.ComponentModel.DataAnnotations;

namespace DevryDeveloperClub.Domain.Dto
{
    public class CreateTagDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Color { get; set; }
    }
}