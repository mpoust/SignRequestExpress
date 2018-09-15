////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Startup.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified:
 * Description: 
 * References: Structure of this project was created using guidance provided from the lynda.com class
 *   "Building and Securing RESTful APIs in ASP.NET Core" by Nate Barbettini.
 *   Other references are cited within the files they are used. 
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandonApi.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignRequestExpressAPI.Filters;

namespace SignRequestExpressAPI
{
    public class Startup
    {
        private readonly int? _httpsPort; // Development only

        /* Configuration changes 
         * in Startup.cs and Program.cs
         * https://joonasw.net/view/aspnet-core-2-configuration-changes
         */
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Order doesnt matter - Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services. 
            services.AddMvc(opt =>
            {
                // Use created exception filter to serialize unhandled exceptions as JSON objects
                opt.Filters.Add(typeof(JsonExceptionFilter));

                // Update the media type to application/ion+json
                var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single(); // current formatter
                opt.OutputFormatters.Remove(jsonFormatter);
                opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter)); // add new formatter supplied with removed
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt); // will use highest version of route if none is requested
            });

            // HTTPS redirect
            //services.AddHttpsRedirection(opt =>
            //{
            //    opt.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    opt.HttpsPort = 44355;
            //});
        }

        // This method gets called by the runtime. Order matters - Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // New way to require https redirection
            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
