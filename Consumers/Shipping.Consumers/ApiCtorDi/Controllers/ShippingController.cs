using JinnDev.Order.Core;
using JinnDev.Order.Data;
using JinnDev.Order.Models;
using JinnDev.OrderDI;
using JinnDev.Shipping.FedEx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Shipping.ApiCtorDi
{
    public class ShippingController : ControllerBase
    {
        IOrderService _orderService;

        public ShippingController(IConfiguration config)
        {
            var connString = config.GetValue<string>("connString");
            var lic = config.GetValue<string>("fedExLicense");
            var orderRepo = new OrderRepo(connString);
            var productRepo = new ProductRepo(connString);
            var custRepo = new CustomerRepo(connString);
            var paySvc = new PaymentService();
            var shipSvc = new FedExShipper(new FedEx.FedExService(lic));
            _orderService = new OrderService(orderRepo, productRepo, custRepo, paySvc, shipSvc);
        }

        [HttpGet]
        [Route("api/orderCtorDi/calculate/{customerId:int}")]
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