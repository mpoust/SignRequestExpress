using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignRequestExpress.Data;
using SignRequestExpress.Services;

namespace SignRequestExpress
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Create HttpClient to be used throughout the application
            services.AddHttpClient("sreApi", c =>
            {
                //c.BaseAddress = new Uri(Configuration.GetSection("ApiSettings").ToString());
                c.BaseAddress = new Uri(Configuration["ApiSettings:ApiUrl"]);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/ion+json"));
            });

            // Add Session services
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                //opt.IdleTimeout = TimeSpan.FromSeconds(20); // Independent of cookie expiration
                opt.IdleTimeout = TimeSpan.FromSeconds(15552000); // Set same as ApiToken lifetime
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Name = ".SRE.Session";
            });

            services.AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
               // .AddSignInManager<SignInManager<IdentityUser>>()
               // Same for role manager
                .AddDefaultTokenProviders();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add Policies
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SalesPolicy",
                    p => p.RequireAuthenticatedUser().RequireRole("Sales"));
                opt.AddPolicy("SignShopPolicy",
                    p => p.RequireAuthenticatedUser().RequireRole("SignShop"));
                opt.AddPolicy("AdministratorPolicy",
                    p => p.RequireAuthenticatedUser().RequireRole("Administrator"));
                opt.AddPolicy("ExecutivePolicy",
                    p => p.RequireAuthenticatedUser().RequireRole("Executive"));
            });

            /* // Alternate authorization policy
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("ClaimPolicy",
                p => p.RequireClaim("FacultyNumber"));
            });
            */

            // Adding Service Interfaces so DefaultService is selected -- not currently being used
            services.AddScoped<ISalesService, DefaultSalesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();            
            // app.UseHttpContextItemsMiddleware(); // what is this?

            app.UseAuthentication(); // Instead of app.UseIdentity();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
