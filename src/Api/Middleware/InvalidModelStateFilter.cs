using System.Net;

using Microsoft.AspNetCore.Mvc.Filters;

namespace Template.Api.Middleware
{
    public class InvalidModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new CustomObjectResult(HttpStatusCode.BadRequest, context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Features.Set(new ModelStateFeature(context.ModelState));
        }
    }
}
