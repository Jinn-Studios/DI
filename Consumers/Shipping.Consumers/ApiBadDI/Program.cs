using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JinnDev.Order.Core;
using JinnDev.OrderBadDI;
using JinnDev.Order.Data.Core;
using JinnDev.Order.Data;
using FedEx;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using JinnDev.Shipping.Core;
using UPS;
using JinnDev.Shipping.FedEx;
using JinnDev.Shipping.UPS;

namespace Shipping.ApiBadDi
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
            var conn = Configuration.GetValue<string>("connString");
            var fedExLic = Configuration.GetValue<string>("fedExLicense");
            var upsLic = Configuration.GetValue<string>("upsLicense");

            containerBuilder.RegisterType<OrderService>().As<IOrderService>();
            containerBuilder.RegisterType<PaymentService>().As<IPaymentService>();
            containerBuilder.RegisterType<UPSService>().As<IUPSService>();

            containerBuilder.Register<IOrderRepo>(x => new OrderRepo(conn));
            containerBuilder.Register<IProductRepo>(x => new ProductRepo(conn));
            containerBuilder.Register<ICustomerRepo>(x => new CustomerRepo(conn));

            // Only some registrars allow you to use the "WhenInjectedInto" paradigm, most don't make it easy...
            containerBuilder.Register<IShipStuff>(x => new FedExShipper(x.Resolve<IFedExService>()));
            containerBuilder.Register<IShipStuff>(x => new UpsShipper(x.Resolve<IUPSService>(), upsLic));

            containerBuilder.Register<IFedExService>(x => new FedExService(fedExLic));
            // Assuming you just changed the signature and haven't done this yet, so it's commented out:
            // containerBuilder.Register<IBuildShips>(x => new ShipperBuilder(new FedExShipper(x.Resolve<IFedExService>()), new UpsShipper(x.Resolve<IUPSService>(), upsLic)));
        }

        public void ConfigureServices(IServiceCollection servicesX)
        {
            servicesX.AddControllers();
        }

        private static void Start()
            => System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", "http://localhost:5000/api/orderBadDi/calculate/3");
    }
}