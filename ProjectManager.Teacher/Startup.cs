using Autofac;
using EmbeddedBlazorContent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectManager.Infrastructure;
using ProjectManager.Ioc;
using ProjectManager.Shared.Common.AppExtensions;
using ProjectManager.Shared.Common.Configuration;
using Radzen;

namespace ProjectManager.Teacher
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureNonBreakingSameSiteCookies();
            ServiceLocator.SetLocatorProvider(services.BuildServiceProvider());
            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(o =>
            {
                o.DetailedErrors = true;
            });
            services.AddPolicies();
            services.AddHttpContextAccessor();
            services.InitAuthentication();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<ComponentRegistrar>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseEmbeddedBlazorContent(typeof(MatBlazor.BaseMatComponent).Assembly);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseMvcWithDefaultRoute();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
