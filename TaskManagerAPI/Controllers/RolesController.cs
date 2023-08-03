using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Core.Domain;
using TaskManager.Service.Filters;
using TaskManager.Service.Interface;

namespace TaskManagerAPI.Controllers
{
    [Route("api/userRoles")]
    [ApiController]
    public class RolesController : BaseApiController
    {

        public RolesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper,
            RoleManager<Role> roleManager, UserManager<User> userManager
            ) : base(repository, logger, mapper)
        {

        }
        
        [HttpGet]
        public IActionResult Index()
        {
            var roles = _repository.RoleManager.GetRoleListAsync();
            return Ok(roles);
        }

        [HttpPost("createRole")]
        public async Task<IActionResult> CreateRole(string role)
        {
            try
            {
                await _repository.RoleManager.CreateRole(role);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateRole)} action {ex}");
                return StatusCode(500, "Internal server error");
            }

        }
        //not used
        [HttpPost("createRoleV2")]
        public async Task<IActionResult> CreateRoleV2([FromBody] Role role)
        {
            try
            {
                await _repository.RoleManager.CreateRoleV2(role);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(CreateRoleV2)} action {ex}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("deleteRole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                await _repository.RoleManager.DeleteRole(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteRole)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("assignRole")]
        [ServiceFilter(typeof(ValidationUserRoles))]
        public async Task<IActionResult> AssignRole(string userId, string roleName) 
        {
            try
            {
                var result = await _repository.RoleManager.AssignRole(userId, roleName);
                if (result.Succeeded) return Ok();
                else return BadRequest();
            }
            catch (Exception ex)
            { 
            _logger.LogError($"Something went wrong in the {nameof(AssignRole)} action {ex}");
            return StatusCode(500, "Internal server error");            
            }
        
        }

        [HttpGet("userRoles")]
        [ServiceFilter(typeof(ValidationUserRoles))]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _repository.RoleManager.GetUserRoles(userId);

            return Ok(roles);
        }

        [HttpPost("removeRole")]
        [ServiceFilter(typeof(ValidationUserRoles))]
        public async Task<IActionResult> DeleteRole(string userId, string roleName)
        {
            try
            {
                var result = await _repository.RoleManager.DeleteRole(userId, roleName);
                if (result.Succeeded) return Ok();
                else return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(DeleteRole)} action {ex}");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}

