using System.ComponentModel.DataAnnotations;

namespace DevryDeveloperClub.Domain.Dto
{
    public class ProjectDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string TechStack { get; set; }
        
        [Required]
        public string GithubLink { get; set; }
    }
}