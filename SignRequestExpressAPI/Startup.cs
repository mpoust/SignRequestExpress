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
            services.AddDbContext<SignAPIContext>(opt => opt.UseInMemoryDatabase("SRETestDB"));

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

            // Gets static information in appsettings.json for the company and create new instance of CompanyInfo with those values
            //  then wraps in an interface called IOptions and puts that into the service container.
            services.Configure<CompanyInfo>(Configuration.GetSection("Info"));
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
            if (env.IsDevelopment())
            {
                var context = app.ApplicationServices.GetRequiredService<SignAPIContext>();
                AddTestData(context);
            }

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

        // Test data for development
        private static void AddTestData(SignAPIContext context)
        {
            context.Accounts.Add(new AccountEntity
            {
                AccountId = Guid.Parse("c529978c-daea-4ed8-a878-d11fda65085a"),
                AccountName = "Brickyard",
                AddedDate = new DateTime(2018,09,15),
                LogoURI = null,
                WebsiteURL = "http://thebrickyard.net/",
                AssociateFK = Guid.Parse("0be59332-bd8f-484d-9f35-0dc17850d23b"), // Nick
                AccountContactFK = Guid.Parse("7689db5d-bfe9-44b6-b39c-c223aeeb15b2"),
                ModifiedDT = new DateTime(2018,09,15,20,18,00)
            });

            context.Accounts.Add(new AccountEntity
            {
                AccountId = Guid.Parse("739133c4-62f6-4693-ac38-d6de239a3745"),
                AccountName = "Hotel Harrington",
                AddedDate = new DateTime(2018, 09, 15),
                LogoURI = null,
                WebsiteURL = "http://www.hotelharringtonpa.com/",
                AssociateFK = Guid.Parse("333e926a-9c93-43f5-a674-fc792de1a499"), // Wyatt
                AccountContactFK = Guid.Parse("719a33c3-58e8-439b-9d97-52f4775aa14f"),
                ModifiedDT = new DateTime(2018, 09, 15, 23, 25, 00)
            });

            context.SaveChanges();
        }

    }
}
