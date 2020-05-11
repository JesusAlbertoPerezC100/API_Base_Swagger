using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Collateral.SupplierPortal.Apis.Core.Interfaces.Gateways.Repositories;
using Collateral.SupplierPortal.Apis.Infrastructure.Data.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Collateral.SupplierPortal.Apis
{
    public class Startup
    {
        private const string Methods = "GET, POST, PUT, DELETE, OPTIONS";
        private const string appsettings = "appsettings.json";
        private const string AllowSpecificOrigins = "_AllowSpecificOrigins";
        private const string AspNetCoreLocalization = "AspNetCoreLocalization";
        private const string ConnectionStringKey = "ConnectionStringCollateral";
        private readonly string ConnectionString;

        public Startup(IWebHostEnvironment env)

        {
            Configuration = new ConfigurationBuilder()
                  .AddJsonFile(appsettings)
                  .AddEnvironmentVariables()
                  .Build();

            ConnectionString = Configuration[ConnectionStringKey];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CollateralContext>(options =>
              options.UseSqlServer(
                  ConnectionString,
                  b => b.MigrationsAssembly(AspNetCoreLocalization)
              ),
              ServiceLifetime.Transient,
              ServiceLifetime.Transient
          );

            services.Load();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Collateral > Supplier Portal Apis", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.WithMethods(Methods);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
            {
                builder
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition");
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Collateral > Supplier Portal Apis V1");
            });

            app.UseSwagger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}