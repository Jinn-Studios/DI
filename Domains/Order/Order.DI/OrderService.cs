﻿using JinnDev.Order.Core;
using JinnDev.Order.Data.Core;
using JinnDev.Order.Data.Enums;
using JinnDev.Order.Models;
using JinnDev.Shipping.Core;
using System.Collections.Generic;

namespace JinnDev.OrderDI
{
    public partial class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IProductRepo _productRepo;
        private readonly ICustomerRepo _customerRepo;

        private readonly IPaymentService _paymentService;
        private readonly IShipStuff _shippingService;

        public OrderService(IOrderRepo orderRepo, IProductRepo productRepo, ICustomerRepo customerRepo, IPaymentService paymentService, IShipStuff shippingService)
        {
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
            _paymentService = paymentService;
            _shippingService = shippingService;
        }

        public OrderModel CalculateOrder(int customerId, List<int> productIDs)
        {
            var customerInfo = _customerRepo.GetCustomerById(customerId).CTM();
            var productInfo = _productRepo.GetProductsByIDs(productIDs).CTMs();

            var order = CalculateOrder(productInfo, customerInfo.Discounts, customerId, 
                orderModel => _shippingService.CalculateStuff(customerInfo.Address.ToShipping(), orderModel));

            order.Status = Order.Enums.OrderStatus.Calculated;
            order.OrderId = _orderRepo.AddOrder(order.CTE());
            return order;
        }

        public bool CreateOrder(int orderId, PaymentInfoModel paymentInfo)
        {
            var order = _orderRepo.GetOrderById(orderId).CTM();

            bool paymentStatus = _paymentService.ProcessPayment(paymentInfo, order.Total);
            if (!paymentStatus) return false;

            order.Status = _orderRepo.UpdateOrderStatus(OrderStatus.Paid).CTM();

            var customerInfo = _customerRepo.GetCustomerById(order.CustomerID).CTM();
            order.Products = _shippingService.CreateShipments(order.Products, customerInfo.Address.ToShipping());

            order.Status = _orderRepo.UpdateOrderStatus(OrderStatus.Shipped).CTM();

            return true;
        }
    }
}