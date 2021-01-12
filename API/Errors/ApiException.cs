using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public record ApiException(int StatusCode, string Message = null, string Details = null);
}
