using JinnDev.Order.Core;
using JinnDev.Order.Data;
using JinnDev.Order.Data.Enums;
using JinnDev.Order.Models;
using System.Collections.Generic;
using System.Linq;
using UPS;

namespace JinnDev.OrderCI
{
    public class OrderServiceUPS : IOrderService
    {
        private readonly string _connString;
        private readonly string _upsLicense;

        public OrderServiceUPS(string connString, string upsLicense)
        {
            _connString = connString;
            _upsLicense = upsLicense;
        }

        public OrderModel CalculateOrder(int customerId, List<int> productIDs)
        {
            //                 vvv
            var customerRepo = new CustomerRepo(_connString);
            //                vvv
            var productRepo = new ProductRepo(_connString);

            var customerInfo = customerRepo.GetCustomerById(customerId).CTM();
            var productInfo = productRepo.GetProductsByIDs(productIDs).CTMs();

            var order = CalculateOrder(productInfo, customerInfo.Discounts, customerInfo.Address, customerId, _upsLicense);
            //              vvv
            var orderRepo = new OrderRepo(_connString);
            order.Status = Order.Enums.OrderStatus.Calculated;
            order.OrderId = orderRepo.AddOrder(order.CTE());
            return order;
        }

        public bool CreateOrder(int orderId, PaymentInfoModel paymentInfo)
        {
            //              vvv
            var orderRepo = new OrderRepo(_connString);
            var order = orderRepo.GetOrderById(orderId).CTM();

            bool paymentStatus = ProcessPayment(paymentInfo, order.Total);
            if (!paymentStatus) return false;

            order.Status = orderRepo.UpdateOrderStatus(OrderStatus.Paid).CTM();

            //                 vvv
            var customerRepo = new CustomerRepo(_connString);
            var customerInfo = customerRepo.GetCustomerById(order.CustomerID);
            var custAddr = customerInfo.Address;
            order.Products.ForEach(x =>
            {
                var upsRequest = new ShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromZIP = 12345,
                    ToZIP = custAddr.ZIP,
                    Weight = x.Weight,
                    CentimetersCubed = x.Height * x.Length * x.Width,
                };
                //x.ShippingGuid = new UPSService().CreateNewShipment(upsRequest, _upsLicense).TrackingNumber;
            });

            order.Status = orderRepo.UpdateOrderStatus(OrderStatus.Shipped).CTM();

            return true;
        }

        public static bool ProcessPayment(PaymentInfoModel paymentInfo, decimal total)
        {
            return true;
        }

        public static OrderModel CalculateOrder(List<ProductModel> products, List<DiscountModel> discounts, AddressModel toAddress, int customerId, string upsLicense)
        {
            var newOrder = new OrderModel
            {
                CustomerID = customerId,
                Products = products,
                NetTotal = products.Sum(x => x.Total / (discounts.FirstOrDefault(y => y.ProductID == x.Id)?.Discount ?? 1))
            };

            //  vvv VVVVV vvv
            var upsRequests = newOrder.Products.Select(x =>
                new ShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromZIP = 12345,
                    ToZIP = toAddress.ZIP,
                    Weight = x.Weight,
                    CentimetersCubed = x.Height * x.Length * x.Width,
                }
            );

            newOrder.Shipping = upsRequests.Sum(x => new UPSService().CalculateShippingFees(x, upsLicense));
            return newOrder;
        }
    }
}