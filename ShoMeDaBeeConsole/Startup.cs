using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoMeDaBee.Internal;

namespace ShoMeDaBee
{
    public class Startup
    {
        public static void Main(string[] args)
            => CreateWebHostBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.SetMinimumLevel(LogLevel.Error); })
                .Build()
                .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            DaBeeSessionManager.Initialize(new ConsoleViewer());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSignalR(route =>
            {
                route.MapHub<DaBeeHubContext>("/dabeehub");
            });
        }
    }
}
