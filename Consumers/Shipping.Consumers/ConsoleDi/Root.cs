using FedEx;
using JinnDev.Order.Core;
using JinnDev.Order.Data;
using JinnDev.Order.Data.Core;
using JinnDev.Shipping.Core;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Shipping.ConsoleDi
{
    public class Root
    {
        public static T GetService<T>()
        {
            if (typeof(T) == typeof(IOrderService))
                return (T)(object)new JinnDev.OrderDI.OrderService(
                    GetService<IOrderRepo>(),
                    GetService<IProductRepo>(),
                    GetService<ICustomerRepo>(),
                    GetService<IPaymentService>(),
                    GetService<IShipStuff>()
                );

            if (typeof(T) == typeof(IPaymentService)) 
                return (T)(object)new PaymentService();

            if (typeof(T) == typeof(IShipStuff))
                return (T)(object)new JinnDev.OrderCI.FedExShipper(GetService<IFedExService>());

            if (typeof(T) == typeof(IFedExService))
                return (T)(object)new FedExService(GetConfig("fedExLicense"));

            if (typeof(T) == typeof(IOrderRepo)) 
                return (T)(object)new OrderRepo(GetConfig("connString"));

            if (typeof(T) == typeof(IProductRepo)) 
                return (T)(object)new ProductRepo(GetConfig("connString"));

            if (typeof(T) == typeof(ICustomerRepo)) 
                return (T)(object)new CustomerRepo(GetConfig("connString"));

            return default(T);
        }

        private static string GetConfig(string configKey)
            => new ConfigurationBuilder()
                .SetBasePath(new DirectoryInfo(Directory.GetCurrentDirectory()).FullName)
                .AddJsonFile("appSettings.json", true, true)
                .Build().GetSection(configKey).Value;
    }
}