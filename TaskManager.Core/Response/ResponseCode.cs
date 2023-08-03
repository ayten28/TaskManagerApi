using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Response
{
    public enum ResponseCode
    {
        Success = 2000,
        DataNotFound = 2001,
        InternalServerError = 2002,
        ValidationError = 2003,
        Failed = 2004,
        UnAuthorized = 2005
    }
}
