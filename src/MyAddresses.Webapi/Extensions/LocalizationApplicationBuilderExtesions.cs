using Microsoft.AspNetCore.Builder;

namespace MyAddresses.Webapi.Extensions
{
    public static class LocalizationApplicationBuilderExtesions
    {
        private static readonly string[] _supportedCultures = new[] { "en", "pt-BR" } ;

        /// <summary>
        /// Configure the localization service for supported cultures by request
        /// </summary>
        /// <param name="app">Application bluider on startup project</param>
        public static void UseAppLocalization(this IApplicationBuilder app) =>
            app.UseRequestLocalization(o => o
                .SetDefaultCulture(_supportedCultures[0])
                .AddSupportedCultures(_supportedCultures)
                .AddSupportedUICultures(_supportedCultures)
            );
    }
}