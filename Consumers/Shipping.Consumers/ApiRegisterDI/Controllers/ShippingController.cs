using JinnDev.Order.Core;
using JinnDev.Order.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Shipping.ApiRegisterDi
{
    public class ShippingController : ControllerBase
    {
        IOrderService _orderService;

        public ShippingController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("api/orderRegisterDi/calculate/{customerId:int}")]
        public string CalculateOrder(int customerId)
        {            
            // The "Entry Point" uses the Service to Command or Query
            OrderModel result = _orderService.CalculateOrder(customerId, new List<int>());

            var lawl = _orderService.CreateOrder(result.OrderId, new PaymentInfoModel());

            // Output for fun:
            return string.Format("Order Total: ${0}, Order Created: {1}", result.NetTotal, lawl);
        }
    }
}