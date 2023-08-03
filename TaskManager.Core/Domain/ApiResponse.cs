using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Response;

namespace TaskManager.Core.Domain
{
    public class ApiResponse<T> : ApiResponse
    {
        public T? Data { get; set; }

        public ApiResponse(T data) : base()
        {
            Data = data;
        }
    }
    public class ApiResponse
    {
        public ResponseCode Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public ApiResponse() : base()
        {
            Code = ResponseCode.Success;
            Message = "Success";
            IsSuccess = true;
        }
    }
}
