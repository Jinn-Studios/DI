using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JinnDev.Order.Core;
using JinnDev.OrderDI;
using JinnDev.Order.Data.Core;
using JinnDev.Order.Data;
using FedEx;
using JinnDev.Shipping.Core;
using JinnDev.Shipping.FedEx;

namespace Shipping.ApiRegisterDi
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public static void Main()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x => x.UseStartup(x => new Program(x.Configuration)).UseUrls("http://*:5000"))
                .Build();
            Start();
            host.Run();
        }

        public Program() { }
        public Program(IConfiguration config) { Configuration = config; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddTransient<IOrderService>(x => new OrderService(
                x.GetService<IOrderRepo>(),
                x.GetService<IProductRepo>(),
                x.GetService<ICustomerRepo>(),
                x.GetService<IPaymentService>(),
                x.GetService<IShipStuff>()));

            var conn = Configuration.GetValue<string>("connString");
            var lic = Configuration.GetValue<string>("fedExLicense");
            services.AddTransient<IOrderRepo>(x => new OrderRepo(conn));
            services.AddTransient<IProductRepo>(x => new ProductRepo(conn));
            services.AddTransient<ICustomerRepo>(x => new CustomerRepo(conn));
            services.AddTransient<IPaymentService>(x => new PaymentService());
            services.AddTransient<IShipStuff>(x => new FedExShipper(x.GetService<IFedExService>()));

            services.AddTransient<IFedExService>(x => new FedExService(lic));
        }

        private static void Start()
            => System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", "http://localhost:5000/api/orderRegisterDi/calculate/3");
    }
}