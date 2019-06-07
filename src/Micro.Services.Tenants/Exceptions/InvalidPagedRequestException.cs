using System;

namespace Micro.Services.Tenants.Exceptions
{
    public class InvalidPagedRequestException : ApplicationException
    {
        public InvalidPagedRequestException(int page, int pageSize) : base($"Invalid paged request page:{page} pageSize:{pageSize}")
        {

        }
    }
}
