using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Shipping.ApiCtorDi
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

            services.AddTransient(x => Configuration);
        }

        private static void Start()
            => System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe", "http://localhost:5000/api/orderCtorDi/calculate/3");
    }
}