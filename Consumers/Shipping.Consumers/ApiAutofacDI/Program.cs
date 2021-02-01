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
using Autofac;
using Autofac.Extensions.DependencyInjection;
using JinnDev.Shipping.Core;
using JinnDev.Shipping.FedEx;

namespace Shipping.ApiAutofacDi
{
    public class Program
    {
        public IConfiguration Configuration { get; }

        public static void Main()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x => x.UseStartup(x => new Program(x.Configuration))
                .UseUrls("http://*:5000")).UseServiceProviderFactory(new AutofacServiceProviderFactory())
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

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.Register<IOrderService>(x => new OrderService(
                x.Resolve<IOrderRepo>(),
                x.Resolve<IProductRepo>(),
                x.Resolve<ICustomerRepo>(),
                x.Resolve<IPaymentService>(),
                x.Resolve<IShipStuff>()));

            var conn = Configuration.GetValue<string>("connString");
            var lic = Configuration.GetValue<string>("fedExLicense");
            containerBuilder.Register<IOrderRepo>(x => new OrderRepo(conn));
            containerBuilder.Register<IProductRepo>(x => new ProductRepo(conn));
            containerBuilder.Register<ICustomerRepo>(x => new CustomerRepo(conn));
            containerBuilder.Register<IPaymentService>(x => new PaymentService());
            containerBuilder.Register<IShipStuff>(x => new FedExShipper(x.Resolve<IFedExService>()));

            containerBuilder.Register<IFedExService>(x => new FedExService(lic));
        }

        public void ConfigureServices(IServiceCollection servicesX)
        {
            servicesX.AddControllers();
        }

        private static void Start()
            => System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", "http://localhost:5000/api/orderAutofacDi/calculate/3");
    }
}