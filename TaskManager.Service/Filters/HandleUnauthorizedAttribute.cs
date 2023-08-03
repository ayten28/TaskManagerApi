using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Service.Filters
{
    public class HandleUnauthorizedAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException) 
            {
                context.Result = new UnauthorizedResult();
                context.ExceptionHandled = true;
            }
        }
    }
}
