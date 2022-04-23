using Microsoft.AspNetCore.Mvc;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Core.Api.Controller;
using OrderApplication.Model.Document.Common.Customer;

namespace OrderApplication.Api.Controllers
{
    public class CustomersController : BaseApiController
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
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
            return ApiDocumentResponse(customerService.InsertOne(customer));
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateCustomerModel customer)
        {
            return ApiDocumentResponse(customerService.ReplaceOne(customer));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            return ApiDocumentResponse(customerService.DeleteOne(id));
        }
    }
}
