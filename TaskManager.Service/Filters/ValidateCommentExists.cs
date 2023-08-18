using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Domain;
using TaskManager.Service.Interface;

namespace TaskManager.Service.Filters
{
    public class ValidateCommentExists : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateCommentExists(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository; _logger = logger;
        }

        public async System.Threading.Tasks.Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT")!;
            var response = new ApiResponse();
            var id = (int)context.ActionArguments[context.ActionArguments.Keys.Where(x => x.Equals("id")).SingleOrDefault()];

            var comment = await _repository.Comments.GetCommentById(id, trackChanges);
            if (comment is null)
            {
                _logger.LogInfo($"Comment with id: {id} doesn't exist in the database.");
       
                response.IsSuccess = false;
                response.Code = Core.Response.ResponseCode.Failed;
                response.Message = $"Comment with id: { id} doesn't exist in the database.";
                context.Result = new BadRequestObjectResult(response);
            }
            else
            {
                context.HttpContext.Items.Add("comment", comment);
                await next();
            }
        }
    }
}
