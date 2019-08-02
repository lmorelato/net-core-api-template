using System.Net;

using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Template.Api.Extensions.ApplicationBuilder;
using Template.Api.Extensions.HostingEnvironment;
using Template.Api.Extensions.ServicesCollection;
using Template.Api.Middleware;
using Template.Core.Profiles;

namespace Template.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext(this.Configuration.GetConnectionString("DefaultConnection"))
                .AddIdentity()
                .AddAuthenticationToken(this.Configuration)
                .AddAuthorization()
                .AddServicesMapping()
                .AddAutoMapper(typeof(AutoMapperProfile))
                .AddMemoryCache()
                .AddSwagger()
                .AddLocalization(o => o.ResourcesPath = "Resources")
                .AddMvcConfigurations()
                .ModifyApiBehaviour();
        }

        public void Configure(IApplicationBuilder application, IHostingEnvironment env)
        {
            application
                .UseExceptionHandler(env.IsTrusted())
                .UseSerilogRequestLogging()
                .UseHsts()
                .UseHttpsRedirection()
                .UseCors(policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader())
                .UseAuthentication()
                .UseMiddleware<ConfigureSessionMiddleware>()
                .UseLocalization()
                .UseSwaggerApi()
                .UseMvc();
        }
    }
}
