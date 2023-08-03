using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Core.Domain;
using TaskManager.Service.Interface;

namespace TaskManager.Service.Filters
{
    public class ValidateTaskExists : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public ValidateTaskExists(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository; _logger = logger;
        }

        public async System.Threading.Tasks.Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT")!;
            var response = new ApiResponse();
            var id = (int)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id") || x.Equals("taskId")).SingleOrDefault()];

            var task = await _repository.Task.GetTaskById(id, trackChanges);
            if (task is null)
            {
                _logger.LogInfo($"Task with id: {id} doesn't exist in the database.");
                //var response = new ObjectResult(new ResponseModel
                //{
                //    StatusCode = 404,
                //    Message = $"Task with id: { id} doesn't exist in the database."
                //});
                response.IsSuccess = false;
                response.Code = Core.Response.ResponseCode.Failed;
                response.Message = $"Task with id: { id} doesn't exist in the database.";
                context.Result = new BadRequestObjectResult(response);
            }
            else
            {
                context.HttpContext.Items.Add("task", task);
                await next();
            }
        }
    }
}
