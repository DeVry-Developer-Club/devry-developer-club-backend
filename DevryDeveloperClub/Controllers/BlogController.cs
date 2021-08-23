using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class BlogController : ApiController<Blog, CreateBlogDto>
    {
        public BlogController(IBaseDbService<Blog> database) : base(database)
        {
        }
    }
}