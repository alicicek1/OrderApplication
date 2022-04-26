using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderApplication.Api.MvcFilters;
using OrderApplication.Business.Service.Abstraction.Mongo;
using OrderApplication.Business.Validation.Order;
using OrderApplication.Core.Api.Controller;
using OrderApplication.Core.Model.Util.AppSettings;
using OrderApplication.Core.Model.Util.Aspect;
using OrderApplication.Model.Document;
using OrderApplication.Model.Document.Common.Order;
using OrderApplication.Model.Util.Request;
using System.Linq;

namespace OrderApplication.Api.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService, IOptions<AppSetting> options) : base(options)
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
            return ApiResponse<Order>(orderService.FindByObjectId(id));
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewOrderModel order)
        {
            return ApiResponse(orderService.Add(order));
        }

        [HttpPut]
        public IActionResult Put([FromBody] UpdateOrderModel order)
        {
            return ApiResponse(orderService.Update(order));
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            return ApiResponse(orderService.DeleteOne(id));
        }

        [HttpPut("ChangeStatus")]
        public IActionResult ChangeStatus(ChangeStatusRequestModel changeStatusRequestModel)
        {
            return ApiResponse(orderService.ChangeStatus(changeStatusRequestModel));
        }
    }
}
