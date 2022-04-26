using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderApplication.Api.MvcFilters;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Validation.Customer;
using OrderApplication.Core.Api.Controller;
using OrderApplication.Core.Model.Util.AppSettings;
using OrderApplication.Core.Model.Util.Aspect;
using OrderApplication.Model.Document.Common.Customer;
using System.Linq;

namespace OrderApplication.Api.Controllers
{
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService customerService;
        public CustomersController(ICustomerService customerService, IOptions<AppSetting> options) : base(options)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return ApiResponse(customerService.GetAllWithoutFilter().ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetAll(string id)
        {
            return ApiResponse(customerService.FindByObjectId(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewCustomerModel customer)
        {
            return ApiResponse(customerService.Add(customer));
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateCustomerModel customer)
        {
            return ApiResponse(customerService.Update(customer));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            return ApiResponse(customerService.DeleteOne(id));
        }
    }
}
