using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using TaskManager.Service.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using IActionFilter = Microsoft.AspNetCore.Mvc.Filters.IActionFilter;
using TaskManager.Core.Domain;

namespace TaskManager.Service.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager _logger;

        public ValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];
           
            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value.ToString()
                .Contains("Dto")).Value;
            
            var response = new ApiResponse();            
           
            if (param is null)
            {
                response.IsSuccess = false;
                response.Code = Core.Response.ResponseCode.ValidationError;
                response.Message = "";
                _logger.LogError($"Object sent from client is null. Controller: {controller}, " +
                    $"action: {action}");

                response.Message = $"Object is null. Controller: {controller}, action: {action}";
                context.Result = new BadRequestObjectResult(response);
                return;
            }
            if (!context.ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Code = Core.Response.ResponseCode.ValidationError;
                var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToList();
                response.Message = "";
                foreach (var e in errors)
                {
                    response.Message += e.Key + ": ";
                    foreach (var aa in e.Errors)
                    {
                        response.Message += aa.ErrorMessage +"; ";
                    }
                }
                //response.Message = context.ModelState;
                _logger.LogError($"Invalid model state for the object. Controller: {controller}, action: {action}");
                context.Result = new UnprocessableEntityObjectResult(response);//context.ModelState);
            }
        }

        public void OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext context) { }
        //public void OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext context)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
