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
using Amics.web.Services;
using System;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using Microsoft.Extensions.FileProviders;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using System.Collections.Generic;

namespace Amics.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IConfiguration Configuration { get; }
        public IFileProvider FileProvider { get; }

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
            services.AddControllers();
            services.AddHealthChecks();            
             
            services.AddMvc().AddNewtonsoftJson();

            // Register reporting services in an application's dependency injection container. 080522
            services.AddDevExpressControls(); // 080522
            // Use the AddMvcCore (or AddMvc) method to add MVC services. 080522
            services.AddMvcCore(); // 080522
                                   // services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();// 080522
            services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();

            //var builder = WebApplication.CreateBuilder(args);
            //IFileProvider? fileProvider = builder.Environment.ContentRootFileProvider;
            //IConfiguration? configuration = builder.Configuration;
            services
             .AddCors(options =>
             {
                 options.AddPolicy("CorsPolicy", builder =>
                 {
                     builder.AllowAnyOrigin();
                     builder.AllowAnyMethod();
                     builder.WithHeaders("Content-Type");
                 });
             });

            //services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
            //    DashboardConfigurator configurator = new DashboardConfigurator();
            //    configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));
            //    //configurator.SetDataSourceStorage(CreateDataSourceStorage());
            //    configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));
            //    //configurator.SetConnectionStringsProvider(new MyDataSourceWizardConnectionStringsProvider());
            //    //configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
            //    return configurator;
            //});

            services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
                DashboardConfigurator configurator = new DashboardConfigurator();
                configurator.SetDashboardStorage(new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));
                configurator.SetDataSourceStorage(CreateDataSourceStorage());
                configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(Configuration));
                configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
                return configurator;
            });

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
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
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
            DevExpress.XtraReports.Web.ClientControls.LoggerService.Initialize(new MyLoggerService());

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

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints => {
                // Maps the dashboard route.
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboard", "DefaultDashboard");
                // Requires CORS policies.
                endpoints.MapControllers().RequireCors("CorsPolicy");
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

        public DataSourceInMemoryStorage CreateDataSourceStorage()
        {
            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

            DashboardJsonDataSource jsonDataSourceSupport = new DashboardJsonDataSource("Support");
            jsonDataSourceSupport.ConnectionName = "jsonSupport";
            jsonDataSourceSupport.RootElement = "Employee";
            dataSourceStorage.RegisterDataSource("jsonDataSourceSupport", jsonDataSourceSupport.SaveToXml());

            DashboardJsonDataSource jsonDataSourceCategories = new DashboardJsonDataSource("Categories");
            jsonDataSourceCategories.ConnectionName = "jsonCategories";
            jsonDataSourceCategories.RootElement = "Products";
            dataSourceStorage.RegisterDataSource("jsonDataSourceCategories", jsonDataSourceCategories.SaveToXml());
            return dataSourceStorage;
        }
        private void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e)
        {
            if (e.ConnectionName == "jsonSupport")
            {
                Uri fileUri = new Uri(FileProvider.GetFileInfo("App_data/Support.json").PhysicalPath, UriKind.RelativeOrAbsolute);
                JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
                jsonParams.JsonSource = new UriJsonSource(fileUri);
                e.ConnectionParameters = jsonParams;
            }
            if (e.ConnectionName == "jsonCategories")
            {
                Uri fileUri = new Uri(FileProvider.GetFileInfo("App_data/Categories.json").PhysicalPath, UriKind.RelativeOrAbsolute);
                JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
                jsonParams.JsonSource = new UriJsonSource(fileUri);
                e.ConnectionParameters = jsonParams;
            }
        }
    }



// ...

//public class MyDataSourceWizardConnectionStringsProvider : IDataSourceWizardConnectionStringsProvider
//    {
//        public Dictionary<string, string> GetConnectionDescriptions()
//        {
//            Dictionary<string, string> connections = new Dictionary<string, string>();

//            // Customize the loaded connections list.  
//            //connections.Add("jsonUrlConnection", "JSON URL Connection");
//            connections.Add("msSqlConnection", "localhost_amicsperaton_Connection");
//            return connections;
//        }

//        public DataConnectionParametersBase GetDataConnectionParameters(string name)
//        {
//            // Return custom connection parameters for the custom connection.
//            if (name == "jsonUrlConnection")
//            {
//                return new JsonSourceConnectionParameters()
//                {
//                    JsonSource = new UriJsonSource(
//                        new Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json"))
//                };
//            }
//            else if (name == "msSqlConnection")
//            {
//                //return new MsSqlConnectionParameters("localhost", "Northwind", "", "", MsSqlAuthorizationType.Windows);
//                // XpoProvider=MSSqlServer;data source=amics-us.c5yec4ayreah.us-east-2.rds.amazonaws.com,2019;user id=amicsmaster2;password=AmicsAt2017;initial catalog=amicsperaton;Persist Security Info=true
//                return new MsSqlConnectionParameters("amics-us.c5yec4ayreah.us-east-2.rds.amazonaws.com,2019", "amicsperaton", "amicsmaster2", "AmicsAt2017", MsSqlAuthorizationType.SqlServer); 
//                //return new MsSqlConnectionParameters("localhost", "Northwind", "", "", MsSqlAuthorizationType.Windows);
//            }
//            throw new System.Exception("The connection string is undefined.");
//        }

//        Dictionary<string, string> IDataSourceWizardConnectionStringsProvider.GetConnectionDescriptions()
//        {
//            throw new NotImplementedException();
//        }
//    }

}
