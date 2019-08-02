#pragma warning disable 1998
using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Template.Api.Extensions.HttpResponse;
using Template.Api.Middleware;
using Template.Core.Exceptions;
using Template.Core.Exceptions.Interfaces;

namespace Template.Api.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        // see @ https://www.strathweb.com/2018/07/centralized-exception-handling-and-request-validation-in-asp-net-core/
        public static IApplicationBuilder UseExceptionHandler(this IApplicationBuilder app, bool isTrusted)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    ProblemDetails problemDetails;
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = errorFeature.Error;

                    if (exception is IKnownException)
                    {
                        var modelState = context.Features.Get<ModelStateFeature>()?.ModelState;
                        problemDetails = HandleKnownException(exception, modelState);
                    }
                    else
                    {
                        problemDetails = HandleException(exception, isTrusted);
                    }

                    context.Response.StatusCode = problemDetails.Status.GetValueOrDefault();
                    context.Response.WriteJson(problemDetails, "application/problem+json");
                });
            });

            return app;
        }

        private static ProblemDetails HandleKnownException(Exception exception, ModelStateDictionary modelState)
        {
            ProblemDetails problemDetails;

            switch (exception)
            {
                case IdentityResultException ex:
                    modelState = modelState ?? new ModelStateDictionary();
                    foreach (var error in ex.Errors)
                    {
                        modelState.TryAddModelError(error.Code, error.Description);
                    }

                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.BadRequest, modelState);
                    break;

                case NotFoundException _:
                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.NotFound, exception.Message);
                    break;
               
                default:
                    problemDetails = ProblemDetailsFactory.New(HttpStatusCode.BadRequest, exception.Message);
                    break;
            }

            return problemDetails;
        }

        private static ProblemDetails HandleException(Exception exception, bool isTrusted)
        {
            if (exception is BadHttpRequestException badHttpRequestException)
            {
                return HandleBadHttpRequestException(badHttpRequestException, isTrusted);
            }

            return HandleInternalServerError(exception, isTrusted);
        }

        private static ProblemDetails HandleInternalServerError(
            Exception exception,
            bool isTrusted)
        {
            var problemDetails = ProblemDetailsFactory.New(
                HttpStatusCode.InternalServerError,
                isTrusted ? exception.Demystify().ToString() : exception.Message);

            return problemDetails;
        }

        private static ProblemDetails HandleBadHttpRequestException(
            BadHttpRequestException exception,
            bool isTrusted)
        {
            var statusCode = (int)typeof(BadHttpRequestException).GetProperty(
                "StatusCode",
                BindingFlags.NonPublic | BindingFlags.Instance).GetValue(exception);

            var problemDetails = ProblemDetailsFactory.New(
                (HttpStatusCode)statusCode,
                isTrusted ? exception.Demystify().ToString() : exception.Message);

            return problemDetails;
        }
    }
}
#pragma warning restore 1998

