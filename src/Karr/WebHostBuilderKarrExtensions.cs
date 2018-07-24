using System;
using Karr.Core;
using Microsoft.AspNetCore.Hosting;

namespace Karr
{
    public static class WebHostBuilderKarrExtensions
    {
        public static IWebHostBuilder UseEndpoint(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(services =>
            {
                // Don't override an already-configured transport
                services.AddKarr();
            });
        }
    }
}
