using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    // localhost/api/projects
    public class ProjectController : ApiController<Project, CreateProjectDto>
    {
        public ProjectController(IBaseDbService<Project> service) : base(service)
        {
        }
    }
}