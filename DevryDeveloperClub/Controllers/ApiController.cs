using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevryDeveloperClub.Infrastructure.Data;
using DevryDeveloperClub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using UnofficialDevryIT.Architecture.Models;

namespace DevryDeveloperClub.Controllers
{
    public abstract class ApiController<TEntity, TEntityDto> : ControllerBase
        where TEntity : class, IEntityWithTypedId<string>, new()
        where TEntityDto : class, new()
    {
        protected readonly IBaseDbService<TEntity> Database;
        protected const string InvalidDataMessage = "Invalid Data";
        
        public ApiController(IBaseDbService<TEntity> database)
        {
            Database = database;
            
        }

        protected bool Validate<T>(T obj, out List<ValidationResult> results)
        {
            results = new();
            var context = new ValidationContext(obj, null, null);
            return Validator.TryValidateObject(obj, context, results, true);
        }

        [HttpGet]
        public async Task<List<TEntity>> Get()
        {
            return await Database.Get();
        }

        [HttpGet]
        [Route("find")]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(InvalidDataMessage);

            var result = await Database.Find(id);

            if (result.Value == null)
                return NotFound(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(List<string>), 400)]
        public async Task<IActionResult> Post(TEntityDto model)
        {
            if (!Validate(model, out List<ValidationResult> errors))
                return BadRequest(errors.Select(x => x.ErrorMessage).ToArray());
            
            var result = await Database.Create(model.CloneTo<TEntity>());

            return CreatedAtAction("Get", GetType().Name.Replace("Controller",""), new { id = result.Value.Id }, result.Value.Id);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(List<string>), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(string id, TEntityDto model)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(InvalidDataMessage);

            if (!Validate(model, out List<ValidationResult> errors))
                return BadRequest(errors.Select(x => x.ErrorMessage).ToArray());

            var result = await Database.Update(model.CloneTo<TEntity>(("Id", id)));

            if (result.Success)
                return NoContent();

            return NotFound(result.ErrorMessage);
        }
        
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(InvalidDataMessage);

            var result = await Database.Delete(id);

            if (result.Success)
                return NoContent();
            
            return NotFound(result.ErrorMessage);
        }
    }
}