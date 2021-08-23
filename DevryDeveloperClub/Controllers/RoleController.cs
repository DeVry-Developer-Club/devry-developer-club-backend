using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.Mongo.Model;
using DevryDeveloperClub.Domain.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevryDeveloperClub.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]s")]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<MongoRole> _roleManager;
        private readonly UserManager<MongoUser> _userManager;
        private readonly ILogger<RoleController> _logger;

        public RoleController(RoleManager<MongoRole> roleManager, UserManager<MongoUser> userManager, ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public Task<List<MongoRole>> Get()
        {
            return Task.FromResult(_roleManager.Roles.ToList());
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(IdentityError[]), 400)]
        public async Task<IActionResult> Create(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);

            if (role != null)
                return BadRequest("Role already exists");

            var result = await _roleManager.CreateAsync(new MongoRole(name));
            
            if(result.Succeeded)
                return Ok();

            return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(IdentityError[]), 400)]
        public async Task<IActionResult> ModifyUserRoles(UpdateUserRoles model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return BadRequest("Invalid user");

            List<MongoRole> roles = new();
            foreach (string roleId in model.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleId);

                if (role == null)
                    return BadRequest($"Invalid Role Id: {roleId}");

                roles.Add(role);
            }

            var result = await _userManager.AddToRolesAsync(user, roles.Select(x=>x.Name));

            if (result.Succeeded)
                return Ok();
            
            _logger.LogError($"Could not add {roles.Select(x=>x.Name)} to {user.UserName}");
            return BadRequest(result.Errors);
        }

        [HttpDelete]
        [Route("user")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(IdentityError[]), 400)]
        public async Task<IActionResult> RemoveRolesFromUser(UpdateUserRoles model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
                return BadRequest("Invalid User");

            List<MongoRole> roles = new();
            foreach (string roleId in model.Roles)
            {
                var role = await _roleManager.FindByNameAsync(roleId);

                if (role == null)
                    return BadRequest($"Invalid Role Id: {roleId}");

                roles.Add(role);
            }

            var result = await _userManager.RemoveFromRolesAsync(user, roles.Select(x=>x.Name));

            if(result.Succeeded)
                return Ok();
            
            _logger.LogError($"Could not remove {roles.Select(x=>x.Name)} from {user.UserName}");
            return BadRequest(result.Errors);
        }
    }
}