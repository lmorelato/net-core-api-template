using System;
using System.Collections.Generic;
using System.Globalization;

using Humanizer;

namespace Template.Shared.Helpers
{
    public static class CultureInfoHelper
    {
        public const string DefaultCultureName = "en-US";

        public static readonly Dictionary<string, CultureInfo> SupportedCultures =
            new Dictionary<string, CultureInfo>(StringComparer.OrdinalIgnoreCase)
            {
                { "en-US", new CultureInfo("en-US") },
                { "pt-BR", new CultureInfo("pt-BR") }
            };

        private static readonly Dictionary<string, CultureInfo> FallbackCulturesMap =
            new Dictionary<string, CultureInfo>(StringComparer.OrdinalIgnoreCase)
            {
                { "en", new CultureInfo("en-US") },
                { "pt", new CultureInfo("pt-BR") }
            };

        public static CultureInfo GetClosestSupportedCulture()
        {
            return GetClosestSupportedCulture(CultureInfo.CurrentCulture.Name);
        }

        public static CultureInfo GetClosestSupportedCulture(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return SupportedCultures[DefaultCultureName];
            }

            if (SupportedCultures.ContainsKey(code))
            {
                return SupportedCultures[code];
            }

            var fallback = TryGetNextSupportedCulture(code);

            return fallback == null ? SupportedCultures[DefaultCultureName] : SupportedCultures[fallback];
        }

        public static string GetClosestSupportedCultureName(string code)
        {
            var culture = GetClosestSupportedCulture(code);
            return culture.Name;
        }

        public static string GetClosestSupportedCultureName()
        {
            var culture = GetClosestSupportedCulture(CultureInfo.CurrentCulture.Name);
            return culture.Name;
        }

        private static string TryGetNextSupportedCulture(string code)
        {
            code = code.Truncate(2);
            return FallbackCulturesMap.ContainsKey(code) ? code : null;
        }
    }
}
