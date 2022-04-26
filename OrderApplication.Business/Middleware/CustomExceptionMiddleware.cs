using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrderApplication.Core.Model.Util.Response;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Business.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {

            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {

            HttpStatusCode httpStatusCode = HttpStatusCode.ServiceUnavailable;
            List<string> Errors = new List<string>();
            if (exception != null && !string.IsNullOrWhiteSpace(exception.Message))
                Errors.Add(exception.Message);

            string errorMessage = "Unexpecteed error occured!";

            context.Response.ContentType = "application/json";


            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exception, true);
                        

            var jsonObject = JsonConvert.SerializeObject(new DataResponse()
            {
                ErrorCode = context.Response.StatusCode.ToString(),
                ErrorMessageList = Errors != null && Errors.Count > 0 ? Errors : new List<string> { errorMessage },
                IsSuccessful = false
            });


            return context.Response.WriteAsync(jsonObject, Encoding.UTF8);
        }
    }
}
