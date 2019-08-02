using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Template.Api.Middleware
{
    // see @ https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/
    public class CustomObjectResult : ObjectResult
    {
        public CustomObjectResult(HttpStatusCode httpStatus) : base(null)
        {
            this.StatusCode = (int)httpStatus;
            this.Value = ProblemDetailsFactory.New(httpStatus);
        }

        public CustomObjectResult(HttpStatusCode httpStatus, string message) : base(null)
        {
            this.StatusCode = (int)httpStatus;
            this.Value = ProblemDetailsFactory.New(httpStatus, message);
        }

        public CustomObjectResult(HttpStatusCode httpStatus, ModelStateDictionary modelState) : base(null)
        {
            this.StatusCode = (int)httpStatus;
            this.Value = ProblemDetailsFactory.New(httpStatus, modelState); 
        }
    }
}
