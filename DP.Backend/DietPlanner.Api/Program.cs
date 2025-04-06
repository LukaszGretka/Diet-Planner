using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DietPlanner.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<EfBenchmark>();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
