using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignRequestExpress.Models;

[assembly: HostingStartup(typeof(SignRequestExpress.Areas.Identity.IdentityHostingStartup))]
namespace SignRequestExpress.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SignRequestExpressContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SignRequestExpressContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<SignRequestExpressContext>();
            });
        }
    }
}