using Aims.Core.Models;
using Amics.Api.Infrastructure;
using Lookup.Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Aims.PartMaster.Services;
using Aims.Core.Services;

namespace Amics.Api
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
            services.AddControllers().AddJsonOptions(options =>
       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
            var connectionString = Configuration.GetValue<string>("ConnectionStrings:LookUpDB");
            services.AddDbContext<AmicsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LookUpDB")));

            var origins = Configuration.GetValue<string>("CORS:origins");
            services
               .AddCors(options => {
                   options.AddPolicy("CorsPolicy", builder => {
                       builder.WithOrigins(origins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                       
                   });
               }).AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Amics 2.0 api",
                    Description = "Amics 2.0 api ",
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

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddMemoryCache();
            services.AddControllersWithViews();
            services.AddApiVersioningConfigured();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IPartmasterService, PartmasterService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<IChangeLocService, ChangeLocationService>();

            services.AddHealthChecks();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddScoped<IAuthorizationPolicy, Infrastructure.AuthorizationPolicy>();
            services.AddScoped<ILookupDbRepositoryService, LookupDbRepositoryService>();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();              
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Amics 2.0");
            });
            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting(); 
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseErrorHandling(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
