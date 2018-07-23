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
            services.AddRouting();
            services.AddKarr();
            services.Configure<EndpointOptions>(options =>
            {
                options.DataSources.Add(new DefaultEndpointDataSource(new[]
                {
                     new MatcherEndpoint((next) => (httpContext) =>
                        {
                             if (httpContext == null)
                            {
                                throw new ArgumentNullException(nameof(httpContext));
                            }

                            var uri = new Uri("http://example.com");

                            return httpContext.ProxyRequest(uri);
                        },
                        RoutePatternFactory.Parse("/"),
                        new RouteValueDictionary(),
                        0,
                        EndpointMetadataCollection.Empty,
                        "Home"),
                    new MatcherEndpoint((next) => (httpContext) =>
                        {
                            if (httpContext == null)
                            {
                                throw new ArgumentNullException(nameof(httpContext));
                            }

                            var uri = new Uri("http://localhost:80/");

                            return httpContext.ProxyRequest(uri);
                        },
                        RoutePatternFactory.Parse("/https"),
                        new RouteValueDictionary(),
                        0,
                        EndpointMetadataCollection.Empty,
                        "Home")
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseGlobalRouting();
            app.UseWebSockets().UseKarr();
            app.UseEndpoint();
        }
    }
}
