using AffittaCamere.RestApiStateless.Helpers.AutoMapperProfiles;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AffittaCamere.RestApiStateless
{
    public class Startup
    {
        private readonly string CorsAllowDefaultOrigin = "CorsAllowDefaultOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // For CORS
            services.AddCors(options =>
            {
                options.AddPolicy(CorsAllowDefaultOrigin, builder =>
                {
                    builder
                        .WithOrigins(
                            "http://localhost:9110", "http://localhost:9120"
                        )
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    ;
                });
            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(CorsAllowDefaultOrigin));

            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // For AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsAllowDefaultOrigin);
            app.UseMvc();
        }
    }
}
