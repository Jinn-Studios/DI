using FedEx;
using JinnDev.Order.Data;
using JinnDev.Order.Data.Enums;
using JinnDev.Order.Models;
using System.Collections.Generic;
using System.Linq;

namespace JinnDev.OrderNoDI
{
    public class OrderService// : IOrderService
    {
        public OrderModel CalculateOrder(int customerId, List<int> productIDs)
        {
            //                                   vvvvvvvvvvvvvvvvvvvvvvvvv
            var customerRepo = new CustomerRepo("sql:username:password:etc");
            var productRepo = new ProductRepo("sql:username:password:etc");

            var customerInfo = customerRepo.GetCustomerById(customerId).CTM();
            var productInfo = productRepo.GetProductsByIDs(productIDs).CTMs();

            var order = CalculateOrder(productInfo, customerInfo.Discounts, customerInfo.Address, customerId);
            var orderRepo = new OrderRepo("sql:username:password:etc");
            order.Status = Order.Enums.OrderStatus.Calculated;
            order.OrderId = orderRepo.AddOrder(order.CTE());
            return order;
        }

        public bool CreateOrder(int orderId, PaymentInfoModel paymentInfo)
        {
            var orderRepo = new OrderRepo("sql:username:password:etc");
            var order = orderRepo.GetOrderById(orderId).CTM();

            bool paymentStatus = ProcessPayment(paymentInfo, order.Total);
            if (!paymentStatus) return false;

            order.Status = orderRepo.UpdateOrderStatus(OrderStatus.Paid).CTM();

            var customerRepo = new CustomerRepo("sql:username:password:etc");
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
                x.ShippingGuid = new FedExService("1F567AAEB479C3B29018F0DC45638DE2").CreateNewShipment(fedExReq);
            });

            order.Status = orderRepo.UpdateOrderStatus(OrderStatus.Shipped).CTM();

            return true;
        }

        public static bool ProcessPayment(PaymentInfoModel paymentInfo, decimal total)
        {
            return true;
        }

        private static OrderModel CalculateOrder(List<ProductModel> products, List<DiscountModel> discounts, AddressModel toAddress, int customerId)
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

            newOrder.Shipping = fedExRequests.Sum(x => new FedExService("1F567AAEB479C3B29018F0DC45638DE2").CalculateShippingFees(x));
            return newOrder;
        }
    }
}