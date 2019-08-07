using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Template.Api.Filters;
using Template.Core.Models.Validators;

namespace Template.Api.Extensions.ServicesCollection
{
    public static partial class ServicesCollectionExtensions
    {
        public static IServiceCollection AddMvcConfigurations(this IServiceCollection services)
        {
            services
                .AddMvc(setup => { setup.Filters.Add(typeof(ModelStateFilter)); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => { fv.RegisterValidatorsFromAssemblyContaining<CredentialsDtoValidator>(); })
                .AddJsonOptions(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                 });

            return services;
        }
    }
}
