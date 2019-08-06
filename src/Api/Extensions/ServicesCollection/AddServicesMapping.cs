using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Template.Core.Services;
using Template.Core.Services.Interfaces;
using Template.Data.Entities.Identity;
using Template.Localization;
using Template.Shared.Session;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddServicesMapping(this IServiceCollection services)
        {
            services.AddScoped<RoleManager<Role>>();
            services.AddScoped<UserManager<User>>();

            services.AddScoped<IUserSession, UserSession>();
            services.AddScoped<ISharedResources, SharedResources>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
