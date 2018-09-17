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
using Microsoft.EntityFrameworkCore;
using SignRequestExpressAPI.Filters;
using SignRequestExpressAPI.Models;
using SignRequestExpressAPI.Entities;
using SignRequestExpressAPI.Services;
using AutoMapper;

namespace SignRequestExpressAPI
{
    public class Startup
    {
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
            // Connecting to database.
            // TODO: Figure out how to connect to real database vs. in-memory
            // services.AddDbContext<SignAPIContext>(opt => opt.UseInMemoryDatabase("SRETestDB"));

            // Connecting to database.
            // TODO: Move connection string to configuration file
            var connection = @"Server=tcp:sign-request-express.database.windows.net,1433;" +
                                "Initial Catalog=SRE-DB;Persist Security Info=False;" +
                                "User ID=mbp3;Password=Mbp934440343;MultipleActiveResultSets=False;" +
                                "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            services.AddDbContext<SignAPIContext>(opt => opt.UseSqlServer(connection));

            // Set up AutoMapper
            services.AddAutoMapper();

            // Add framework services. 
            services.AddMvc(opt =>
            {
                // Use created exception filter to serialize unhandled exceptions as JSON objects
                opt.Filters.Add(typeof(JsonExceptionFilter));
                // Use created result filter to rewrite links before they are sent as a response
                opt.Filters.Add(typeof(LinkRewritingFilter));

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

            // Gets static information in appsettings.json for the company and create new instance of CompanyInfo with those values
            //  then wraps in an interface called IOptions and puts that into the service container.
            services.Configure<CompanyInfo>(Configuration.GetSection("Info"));

            // Adding Service Interfaces so Default service is selected
            services.AddScoped<IAccountService, DefaultAccountService>();
            services.AddScoped<IAccountContactService, DefaultAccountContactService>();
        }

        // This method gets called by the runtime. Order matters - Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add some test data in development -- goes with services.AddDbContext above
            // TODO: connect to real database and gather data that way
            /*
            if (env.IsDevelopment())
            {
                var context = app.ApplicationServices.GetRequiredService<SignAPIContext>();
                AddTestData(context);
            }
            */

            // New way to require https redirection
            app.UseHttpsRedirection();

            // Add the HSTS header - for supported browsers this won't even allow an attempt to connect over plain HTTP
            app.UseHsts(opt =>
            {
                opt.MaxAge(days: 360);
                opt.IncludeSubdomains();
                opt.Preload();
            });
            app.UseMvc();
        }
    }
}
