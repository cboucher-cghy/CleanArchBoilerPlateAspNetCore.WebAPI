using CleanArchBoilerPlateAspNetCore.Infra.Data.DbContexts;
using CleanArchBoilerPlateAspNetCore.Infra.IoC;
using CleanArchBoilerPlateAspNetCore.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;

namespace CleanArchBoilerPlateAspNetCore.WebAPI
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
            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            //services.AddAuthorization();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CleanArchBoilerPlateAspNetCoreConnection"));
            });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true; // Ensure it's the latest version used when nothing is specified in the URL
                o.DefaultApiVersion = new ApiVersion(1, 0);

                // Allow to hardcode the "v1" in all swagger request in the UI.
                // Source: https://stackoverflow.com/questions/62173905/why-does-swagger-need-a-version-requestparameter-when-the-api-version-is-in-the
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                });

            //Register the ClaimsTransformer here (https://stackoverflow.com/questions/58825446/asp-net-core-3-0-windows-authentication-with-custom-role-based-authorization)
            //services.AddScoped<IClaimsTransformation, ClaimsTransformer>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanArchBoilerPlateAspNetCore.WebAPI", Version = "v1", Description = "CleanArchBoilerPlateAspNetCore Web API", });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            services.AddHttpContextAccessor();

            //Registring services from infra
            DependencyContainer.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseGlobalErrorHandlingMiddleware();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchBoilerPlateAspNetCore.WebAPI v1");
                    //options.RoutePrefix = string.Empty; // Get Swagger at root URL instead of "/swagger"
                });
            }
            else
            {
                app.UseGlobalErrorHandlingMiddleware();
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
