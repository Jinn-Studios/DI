using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Shipping.ApiNoDi
{
    public class Program
    {
        public static void Main()
        {
            using var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x => x.UseStartup<Program>().UseUrls("http://*:5000"))
                .Build();
            Start();
            host.Run();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        private static void Start()
            => System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", "http://localhost:5000/api/orderNoDi/calculate/3");
    }
}