using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

//using DevExpress.AspNetCore;
//using DevExpress.AspNetCore.Reporting;
//using DevExpress.XtraReports.Web.Extensions;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SpaServices.AngularCli;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using DocumentViewerApp.Services;

using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.Services;
using Amics.web.Services;
using System;

namespace Amics.web
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

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Amics 2.0",
                    Description = "Amics 2.0",
                    //Todo
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Example Contact",
                    //    Url = new Uri("https://example.com/contact")
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Example License",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
            });
            services.AddControllersWithViews();
            services.AddHealthChecks();            
             
            services.AddMvc().AddNewtonsoftJson();

            // Register reporting services in an application's dependency injection container. 080522
            services.AddDevExpressControls(); // 080522
            // Use the AddMvcCore (or AddMvc) method to add MVC services. 080522
            services.AddMvcCore(); // 080522
                                   // services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();// 080522
            //services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
            services.AddScoped<IReportProvider, CustomReportProvider>();

            services.ConfigureReportingServices(configurator => {
                configurator.ConfigureReportDesigner(designerConfigurator => {
                    designerConfigurator.RegisterDataSourceWizardConfigFileConnectionStringsProvider();
                });
                configurator.ConfigureWebDocumentViewer(viewerConfigurator => {
                    viewerConfigurator.UseCachedReportSourceBuilder();
                });
            });

            services.AddCors(options => {
                options.AddPolicy("AllowCorsPolicy", builder => {
                    // Allow all ports on local host.
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
                    builder.WithHeaders("Content-Type");
                });
            });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DevExpress.XtraReports.Configuration.Settings.Default.UserDesignerOptions.DataBindingMode = DevExpress.XtraReports.UI.DataBindingMode.Expressions;
            //DevExpress.XtraReports.Web.ClientControls.LoggerService.Initialize(new MyLoggerService());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Amics 2.0"); 
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Initialize reporting services. 080522
            app.UseDevExpressControls(); // 080522

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseDevExpressControls();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowCorsPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
