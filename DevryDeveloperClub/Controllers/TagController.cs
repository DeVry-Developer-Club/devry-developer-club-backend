using System.Collections.Generic;
using System.Threading.Tasks;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using DevryDeveloperClub.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevryDeveloperClub.Controllers
{
    // TODO: Add Authentication 
    
    
    [ApiController]
    [Route("api/[controller]")]
    // localhost/api/tag
    public class TagController : ControllerBase
    {
        private readonly IApplicationDbContext _context;
        private readonly ITagService _service;

        public TagController(IApplicationDbContext context, ITagService service)
        {
            _context = context;
            _service = service;
        }

        /// <summary>
        /// Retrieve a list of tags from the database
        /// </summary>
        /// <returns>
        /// List of <see cref="Tag"/>s that are currently in the database
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Tag>), 200)]
        // localhost/api/tag/tags
        public async Task<List<Tag>> Get()
        {
            return await _service.Get();
        }

        /// <summary>
        /// Locate a particular tag with an existing ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Result from database or 404 (Not Found) if it doesn't exist
        /// </returns>
        [HttpGet]
        [Route("find")]
        [ProducesResponseType(typeof(Tag), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.Find(id);

            if (result.Value == null)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        /// <summary>
        /// Create the 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Post(string name, string color)
        {
            var result = await _service.Create(name, color);
            return Ok(result.Value);
        }
        
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(string id, string name, string color)
        {
            var result = await _service.Update(id, name, color);

            if (result.Success)
                return Ok();

            return NotFound(result.ErrorMessage);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _service.Delete(id);

            if (result.Success) 
                return Ok();

            return NotFound(result.ErrorMessage);
        }
    }
}