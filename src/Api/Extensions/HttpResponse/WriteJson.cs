using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Template.Api.Extensions.HttpResponse
{
    public static class HttpResponseExtensions
    {
        public static void WriteJson<T>(this Microsoft.AspNetCore.Http.HttpResponse response, T obj, string contentType = null)
        {
            response.ContentType = contentType ?? "application/json";

            using (var writer = new HttpResponseStreamWriter(response.Body, Encoding.UTF8))
            {
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    jsonWriter.CloseOutput = false;
                    jsonWriter.AutoCompleteOnClose = false;

                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, obj);
                }
            }
        }
    }
}
