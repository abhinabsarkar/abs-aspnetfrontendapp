using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetfrontendapp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Add JSON configuration provider
                // Refer https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#json-configuration-provider
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    /* 
                       The below code removes all of the sources and ChainedConfigurationSource specifically that leads to the error below when containerized
                       "Unable to bind to https://localhost:5000 on the IPv6 loopback interface"
                    */
                    
                    //config.Sources.Clear();  

                    // Altternative solution is remove the JSON configuration only
                    config.Sources.RemoveAt(1);

                    var env = hostingContext.HostingEnvironment;

                    /*
                        In the ConfigurationBuilder, we're telling ASP.NET to get its app settings from appsettings.json, then from a file 
                        named config/appsettings.json. If a value exists in multiple places, the last one that gets applied wins.                    
                    */

                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                          // In the k8s, the appsettings.json will be placed inside config folder using config map
                          .AddJsonFile("config/appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
