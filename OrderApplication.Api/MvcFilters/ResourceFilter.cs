using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using OrderApplication.Core.Extension;
using OrderApplication.Core.Model.Util.Response;
using OrderApplication.Core.Tool;
using OrderApplication.Core.Validation;
using OrderApplication.Model.Document;
using OrderApplication.Model.Document.Common.Customer;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace OrderApplication.Api.MvcFilters
{
    public class ResourceFilter : Attribute, IFilterFactory, IFilterMetadata, IOrderedFilter, IResourceFilter
    {
        private Type _validatorType;
        private DataResponse result { get; set; } = new DataResponse();
        private ObjectFactory _factory;
        public object[] Arguments { get; set; }
        public Type ImplementationType { get; }
        public int Order { get; set; }

        public bool IsReusable { get; set; }

        public ResourceFilter(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new System.Exception("IsAssignableFrom format error!");
            }

            _validatorType = validatorType;
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //throw new System.NotImplementedException();
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType, ServiceTool.ServiceProvider);
            var modelType = _validatorType.BaseType.BaseType.GetGenericArguments()[0];

            var request = context.HttpContext.Request;

            if ((request.Method == HttpMethod.Post.ToString() ||
               request.Method == HttpMethod.Put.ToString()) &&
               request.Body != null)
            {
                ArgumentNullException.ThrowIfNull(request.Body);

                using (var reader = new StreamReader(request.Body))
                {
                    var bodyRequest = reader.ReadToEnd();


                    object model = Newtonsoft.Json.JsonConvert.DeserializeObject(bodyRequest);
                    ArgumentNullException.ThrowIfNull(model);

                    result = ValidationTool.Validate(validator, model);
                    if (!result.IsSuccessful)
                    {
                        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        throw new Exception(string.Join("-", result.ErrorMessageList));
                    }
                }
            }



        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ResourceFilter(_validatorType);
        }
    }
}
