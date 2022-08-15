using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;  
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace Amics.ReportingServer
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
            // Register reporting services in an application's dependency injection container.
            services.AddDevExpressControls();
            // Use the AddMvcCore (or AddMvc) method to add MVC services.
            services.AddMvc();
            //services.AddNewtonsoftJson();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
             
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });

            app.UseDevExpressControls();

        }
    }
}
