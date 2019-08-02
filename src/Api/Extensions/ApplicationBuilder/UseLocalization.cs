﻿using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Template.Shared.Helpers;

namespace Template.Api.Extensions.ApplicationBuilder
{
    public static partial class ApplicationBuilderExtensions
    {
        // https://www.jeffogata.com/asp-net-core-localization-culture/
        public static IApplicationBuilder UseLocalization(this IApplicationBuilder app)
        {
            var supportedCultures = CultureInfoHelper.SupportedCultures.Values.ToList();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(CultureInfoHelper.DefaultCultureName),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }
    }
}