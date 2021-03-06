using Application;
using Common;
using Infrastructure;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Presentation.Areas.Identity;
using Presentation.Areas.Identity.Data;
using Presentation.Orders.Services.Commands.SaveApplicationUser;
using Presentation.Orders.Services.Queries.GetApplicationUser;
using Presentation.Properties;
using Presentation.ShoppingCarts.Services.Queries;

[assembly: AspMvcViewLocationFormat(@"~/{1}/Views/{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~/Shared/Views/{0}.cshtml")]
[assembly: AspMvcPartialViewLocationFormat(@"~/Shared/Views/{0}.cshtml")]


namespace Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
                o.AreaPageViewLocationFormats.Clear();
                o.AreaPageViewLocationFormats.Add("/Shared/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddCommonServiceCollection();

            services.AddPersistenceServiceCollection(Configuration[Resources.ConnectionStringKey.Replace("__", ":")]);

            services.AddApplicationServiceCollection();

            services.AddInfrastructureServiceCollection();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration[Resources.ApplicationConnectionStringKey.Replace("__", ":")]));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(CartIdProvider.Execute);
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IGetApplicationUserDetails, GetApplicationUserDetails>();
            services.AddScoped<IGetApplicationUserId, GetApplicationUserId>();
            services.AddScoped<ISaveApplicationUserDetails, SaveApplicationUserDetails>();

            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default",
                    "{controller=Home}/{action=Index}/{Id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}