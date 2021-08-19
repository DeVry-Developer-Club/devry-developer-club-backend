using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.XPath;
using DevryDeveloperClub.Domain.Dto;
using DevryDeveloperClub.Domain.Models;
using DevryDeveloperClub.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace DevryDeveloperClub.Controllers
{
    // TODO: Add Authentication 

    [ApiController]
    [Route("api/[controller]")]
    // localhost/api/tag
    public class TagController : ControllerBase
    {
        private readonly IBaseDbService<Tag> _service;
        private const string InvalidDataMessage = "Invalid Data";
        
        public TagController(IBaseDbService<Tag> service)
        {
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
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(InvalidDataMessage);
            
            var result = await _service.Find(id);

            if (result.Value == null)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        /// <summary>
        /// Create the 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post(CreateTagDto model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Color))
                return BadRequest(InvalidDataMessage);
            
            var result = await _service.Create(new(){Name = model.Name, ColorValue = model.Color});
            return CreatedAtAction("Get", new { id = result.Value.Id }, result.Value.Id);
        }
        
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(string id, string name, string color)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(color))
                return BadRequest(InvalidDataMessage);
            
            var result = await _service.Update(new(){Id = id,Name = name,ColorValue = color});

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
            if (string.IsNullOrEmpty(id))
                return BadRequest(InvalidDataMessage);
            
            var result = await _service.Delete(id);

            if (result.Success) 
                return NoContent();

            return NotFound(result.ErrorMessage);
        }
    }
}