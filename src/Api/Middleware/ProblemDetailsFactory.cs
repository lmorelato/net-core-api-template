using System;
using System.Linq;
using System.Net;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Template.Api.Middleware
{
    public static class ProblemDetailsFactory
    {
        public static ProblemDetails New(HttpStatusCode httpStatusCode, ModelStateDictionary modelState)
        {
            var hasError = modelState != null && !modelState.IsValid;
            var problemDetails = hasError ?
                                     new ValidationProblemDetails(modelState) :
                                     new ProblemDetails();

            SetValues(problemDetails, httpStatusCode, GetProblemDetailDescription(modelState));
            return problemDetails;
        }

        public static ProblemDetails New(HttpStatusCode httpStatusCode, string detail)
        {
            var problemDetails = new ProblemDetails();
            SetValues(problemDetails, httpStatusCode, detail);
            return problemDetails;
        }

        public static ProblemDetails New(HttpStatusCode httpStatusCode)
        {
            var problemDetails = new ProblemDetails();
            SetValues(problemDetails, httpStatusCode, null);
            return problemDetails;
        }

        private static void SetValues(ProblemDetails problemDetails, HttpStatusCode httpStatusCode, string detail)
        {
            problemDetails.Status = (int)httpStatusCode;
            problemDetails.Title = httpStatusCode.ToString().Titleize();
            problemDetails.Instance = $"urn:api:{httpStatusCode.ToString().ToLower()}:{Guid.NewGuid()}";
            problemDetails.Detail = detail;
        }

        private static string GetProblemDetailDescription(ModelStateDictionary modelState)
        {
            if (modelState == null || modelState.IsValid)
            {
                return null;
            }

            var errorEntries = modelState.Where(e => e.Value.Errors.Count > 0).ToList();
            if (errorEntries.Count == 1 &&
                errorEntries.First().Value.Errors.Count == 1 &&
                errorEntries.First().Key == string.Empty)
            {
                return errorEntries.First().Value.Errors.First().ErrorMessage;
            }

            return "See errors for details";
        }
    }
}
