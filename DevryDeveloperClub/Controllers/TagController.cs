using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    // TODO: Add Authentication 

    [ApiController]
    [Route("api/[controller]s")]
    // localhost/api/tags
    public class TagController : ApiController<Tag, CreateTagDto>
    {
        public TagController(IBaseDbService<Tag> service) : base(service)
        {
        }
    }
}