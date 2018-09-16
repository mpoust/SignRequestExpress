////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Program.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified:
 * Description: 
 * References:
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SignRequestExpressAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            /*
            var host = CreateWebHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                services.GetRequiredService<SignAPIContext>();
            }

            host.Run();
            */
        }

        /* Same configuration changes referenced in startup.
         * 
         * For configuration this registers the following configuration sources:
         *      - appsettings.json
         *      - appsettings.environments.json
         *      - User secrets for the current assembly
         *      - Environment variables
         *      - Command-line arguments (if not null)
         *      
         * Note: Can call ConfigureAppConfiguration again after creating the default builder. 
         *  This will not replace the default configuration, but will add on top of it. 
         */
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
