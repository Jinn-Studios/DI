using FedEx;
using JinnDev.Order.Core;
using JinnDev.Order.Data;
using JinnDev.Order.Data.Enums;
using JinnDev.Order.Models;
using System.Collections.Generic;
using System.Linq;

namespace JinnDev.OrderCI
{
    public class OrderService : IOrderService
    {
        private readonly string _connString;
        private readonly string _fedExLicense;

        public OrderService(string connString, string fedExLicense)
        {
            _connString = connString;
            _fedExLicense = fedExLicense;
        }

        public OrderModel CalculateOrder(int customerId, List<int> productIDs)
        {
            var customerRepo = new CustomerRepo(_connString);
            var productRepo = new ProductRepo(_connString);

            var customerInfo = customerRepo.GetCustomerById(customerId).CTM();
            var productInfo = productRepo.GetProductsByIDs(productIDs).CTMs();

            var order = CalculateOrder(productInfo, customerInfo.Discounts, customerInfo.Address, customerId, _fedExLicense);
            var orderRepo = new OrderRepo(_connString);
            order.Status = Order.Enums.OrderStatus.Calculated;
            order.OrderId = orderRepo.AddOrder(order.CTE());
            return order;
        }

        public bool CreateOrder(int orderId, PaymentInfoModel paymentInfo)
        {
            var orderRepo = new OrderRepo(_connString);
            var order = orderRepo.GetOrderById(orderId).CTM();

            bool paymentStatus = ProcessPayment(paymentInfo, order.Total);
            if (!paymentStatus) return false;

            order.Status = orderRepo.UpdateOrderStatus(OrderStatus.Paid).CTM();

            var customerRepo = new CustomerRepo(_connString);
            var customerInfo = customerRepo.GetCustomerById(order.CustomerID);
            var custAddr = customerInfo.Address;
            order.Products.ForEach(x =>
            {
                var fedExReq = new FedExShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromAddress = new FedExAddress { StreetAddress = "1234 St", City = "OKC", State = "OK", Country = "US", AreaCode = 12345 },
                    ToAddress = new FedExAddress { StreetAddress = custAddr.Street, City = custAddr.City, State = custAddr.State, Country = "US", AreaCode = custAddr.ZIP },
                    Weight = x.Weight,
                    Height = x.Height,
                    Length = x.Length,
                    Width = x.Width,
                };
                //vvvvvvvvVVVV
                x.ShippingGuid = new FedExService(_fedExLicense).CreateNewShipment(fedExReq);
            });

            order.Status = orderRepo.UpdateOrderStatus(OrderStatus.Shipped).CTM();

            return true;
        }

        public static bool ProcessPayment(PaymentInfoModel paymentInfo, decimal total)
        {
            return true;
        }

        public static OrderModel CalculateOrder(List<ProductModel> products, List<DiscountModel> discounts, AddressModel toAddress, int customerId, string fedExLicense)
        {
            var newOrder = new OrderModel
            {
                CustomerID = customerId,
                Products = products,
                NetTotal = products.Sum(x => x.Total / (discounts.FirstOrDefault(y => y.ProductID == x.Id)?.Discount ?? 1))
            };

            //  vvv VVVVV vvv
            var fedExRequests = newOrder.Products.Select(x =>
                new FedExShippingRequest
                {
                    DeclaredValue = x.Total,
                    FromAddress = new FedExAddress { StreetAddress = "1234 St", City = "OKC", State = "OK", Country = "US", AreaCode = 12345 },
                    ToAddress = new FedExAddress { StreetAddress = toAddress.Street, City = toAddress.City, State = toAddress.State, Country = "US", AreaCode = toAddress.ZIP },
                    Weight = x.Weight,
                    Height = x.Height,
                    Length = x.Length,
                    Width = x.Width,
                }
            );

            newOrder.Shipping = fedExRequests.Sum(x => new FedExService(fedExLicense).CalculateShippingFees(x));
            return newOrder;
        }
    }
}