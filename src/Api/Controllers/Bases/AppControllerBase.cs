using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Template.Api.Middleware;

namespace Template.Api.Controllers.Bases
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class AppControllerBase : ControllerBase
    {
        protected CustomObjectResult CustomResult(HttpStatusCode httpStatusCode)
        {
            return new CustomObjectResult(httpStatusCode);
        }

        protected CustomObjectResult CustomResult(HttpStatusCode httpStatusCode, string message)
        {
            return new CustomObjectResult(httpStatusCode, message);
        }

        protected CustomObjectResult CustomResult(HttpStatusCode httpStatusCode, ModelStateDictionary modelState)
        {
            return new CustomObjectResult(HttpStatusCode.BadRequest, modelState);
        }

        protected new CustomObjectResult BadRequest()
        {
            return new CustomObjectResult(HttpStatusCode.BadRequest);
        }

        protected CustomObjectResult BadRequest(string message)
        {
            return new CustomObjectResult(HttpStatusCode.BadRequest, message);
        }

        protected new CustomObjectResult BadRequest(ModelStateDictionary modelState)
        {
            return new CustomObjectResult(HttpStatusCode.BadRequest, modelState);
        }

        protected new CustomObjectResult NotFound()
        {
            return new CustomObjectResult(HttpStatusCode.NotFound);
        }

        protected CustomObjectResult NotFound(string message)
        {
            return new CustomObjectResult(HttpStatusCode.NotFound, message);
        }
    }
}