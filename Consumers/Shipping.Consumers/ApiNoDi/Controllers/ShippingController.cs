using JinnDev.Order.Core;
using JinnDev.Order.Data;
using JinnDev.Order.Models;
using JinnDev.OrderDI;
using JinnDev.Shipping.FedEx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Shipping.ApiNoDi
{
    public class ShippingController : ControllerBase
    {
        IConfiguration _config;

        public ShippingController(IConfiguration config)
        { _config = config; }

        [HttpGet]
        [Route("api/orderNoDi/calculate/{customerId:int}")]
        public string CalculateOrder(int customerId)
        {
            var connString = _config.GetValue<string>("connString");
            var lic = _config.GetValue<string>("fedExLicense");
            var orderRepo = new OrderRepo(connString);
            var productRepo = new ProductRepo(connString);
            var custRepo = new CustomerRepo(connString);
            var paySvc = new PaymentService();
            var shipSvc = new FedExShipper(new FedEx.FedExService(lic));
            var orderService = new OrderService(orderRepo, productRepo, custRepo, paySvc, shipSvc);

            // The "Entry Point" uses the Service to Command or Query
            OrderModel result = orderService.CalculateOrder(customerId, new List<int>());

            var lawl = orderService.CreateOrder(result.OrderId, new PaymentInfoModel());

            // Output for fun:
            return string.Format("Order Total: ${0}, Order Created: {1}", result.NetTotal, lawl);
        }
    }
}