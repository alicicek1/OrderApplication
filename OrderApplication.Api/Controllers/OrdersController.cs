using Microsoft.AspNetCore.Mvc;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Core.Api.Controller;
using OrderApplication.Model.Document;

namespace OrderApplication.Api.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return ApiResponse(orderService.GetAllWithoutFilter().ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetAll(string id)
        {
            return ApiResponse(orderService.FindByObjectId(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            return ApiResponse(orderService.InsertOne(order));
        }

        [HttpPut]
        public IActionResult Put([FromBody] Order order)
        {
            return ApiResponse(orderService.ReplaceOne(order));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            return ApiResponse(orderService.DeleteOne(id));
        }
    }
}
