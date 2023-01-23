using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FsccProxy
{
    //https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService(options =>
                {
                    options.ServiceName = "fscc proxy";
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}