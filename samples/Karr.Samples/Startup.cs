using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karr.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matchers;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.DependencyInjection;

namespace Karr.Samples
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private static readonly byte[] _homePayload = Encoding.UTF8.GetBytes("Global Routing sample endpoints:" + Environment.NewLine + "/plaintext");
        private static readonly byte[] _helloWorldPayload = Encoding.UTF8.GetBytes("Hello, World!");

        public void ConfigureServices(IServiceCollection services)
        {
            // Set application configuration
            services.AddRouting();
            services.AddKarr(); // TODO this may just be calling AddProxy

            services.Configure<EndpointOptions>(options =>
            {
                options.DataSources.Add(new DefaultEndpointDataSource(new Endpoint[]
                {
                    new ProxyEndpoint(
                        RoutePatternFactory.Parse("/foo"),
                        new RouteValueDictionary(),
                        0,
                        EndpointMetadataCollection.Empty,
                        "ProxyEndpoint via data sources",
                        new Uri("http://example.com")),
                    new MatcherEndpoint(
                        (next) => (httpContext) => {
                            httpContext.Response.Headers["FinalHeader"] = "what";
                            return httpContext.ProxyRequest(new Uri("http://example.com"));
                        },
                        RoutePatternFactory.Parse("/bar"),
                        new RouteValueDictionary(),
                        0,
                        EndpointMetadataCollection.Empty,
                        "Matcher endpoint with proxy.")
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseGlobalRouting();
            app.UseEndpoint(builder => {
                builder.AddProxyEndpoint(
                    RoutePatternFactory.Parse("/"),
                    new RouteValueDictionary(),
                    0,
                    EndpointMetadataCollection.Empty,
                    "ProxyEndpoint via Builder",
                    new Uri("http://example.com"));/*AddAuthPolicy("A sled gang")*/ 
                // TODO make AddProxyEndpoint return a builder to add policies onto
            });
        }
    }
}
