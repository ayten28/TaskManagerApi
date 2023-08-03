using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Response
{
    public static class ResponseMessage
    {
        public static readonly Dictionary<ResponseCode, string> ResponseMessages
            = new Dictionary<ResponseCode, string>
        {
            { ResponseCode.Success, "Success" },
            { ResponseCode.DataNotFound, "Data could not be found" },
            { ResponseCode.InternalServerError, "Internal server error occured" },
            { ResponseCode.ValidationError, "Validation error occured" },
            { ResponseCode.Failed, "Failed" }
        };
    }
}
