using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrderApplication.Core.Model.Util.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrderApplication.Business.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }

        private void ThrowException(HttpContext context, string errorString)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            context.Response.ContentType = "application/json; charset=utf-8";

            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("access-control-expose-headers", "Authorization,api-supported-versions,X-Total-Count");
            context.Response.Headers.Add("api-supported-versions", "1.0");

            string jsonObject = JsonConvert.SerializeObject(new DataResponse { ErrorMessageList = new List<string> { errorString } });
            context.Response.WriteAsync(jsonObject, Encoding.UTF8).Wait();
        }
    }
}
