using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Core.Domain;
using TaskManager.Service.Interface;
using Task = System.Threading.Tasks.Task;

namespace TaskManager.Service.Filters
{
    public class ValidationUserRoles : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public ValidationUserRoles(ILoggerManager logger, IRepositoryManager repository, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _logger = logger;
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("POST")!;

            var id = (string)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("userId")).SingleOrDefault()];
            var name = (string)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("roleName")).SingleOrDefault()];


            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                var response = new ObjectResult(new ResponseModel
                {
                    StatusCode = 404,
                    Message = $"User with id: { id} doesn't exist in the database."
                });
                context.Result = response;
            }
            else
            {
                context.HttpContext.Items.Add("user", user);
                var role = await _roleManager.FindByNameAsync(name);
                if (role is null)
                {
                    _logger.LogInfo($"Role with name: {name} doesn't exist in the database.");
                    var response = new ObjectResult(new ResponseModel
                    {
                        StatusCode = 404,
                        Message = $"Role with name: { name } doesn't exist in the database."
                    });
                    context.Result = response;
                }
                else
                {
                    context.HttpContext.Items.Add("role", role);
                    await next();
                }
            }
        }
    }
}
