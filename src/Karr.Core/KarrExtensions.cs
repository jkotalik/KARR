using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Karr.Core
{
    public static class KarrExtensions
    {
        private const string GlobalRoutingRegisteredKey = "__GlobalRoutingMiddlewareRegistered";

        public static IApplicationBuilder UseKarr(this IApplicationBuilder builder)
        {
            builder.UseGlobalRouting();
            builder.UseWebSockets();
            // Idea #1 to implement this
            // call app.UseProxyEndpoint() and create custom endpoints which proxies the request
            return builder.UseProxyEndpoint("http://example.com");
        }

        public static IApplicationBuilder UseProxyEndpoint(this IApplicationBuilder builder, string baseUri)
        {
            if (!builder.Properties.TryGetValue(GlobalRoutingRegisteredKey, out _))
            {
                throw new InvalidOperationException("Must register GlobalRouting middleware before proxy endpoint");
            }

            // TODO populate uris via endpoints
            var uri = new Uri(baseUri);

            var options = new KarrOptions
            {
                Scheme = uri.Scheme,
                Host = new HostString(uri.Authority)
            };

            return builder.UseMiddleware<KarrMiddleware>(Options.Create(options));
        }

        public static async Task ProxyRequest(this HttpContext context, Uri destinationUri)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (destinationUri == null)
            {
                throw new ArgumentNullException(nameof(destinationUri));
            }

            if (context.WebSockets.IsWebSocketRequest)
            {
                await context.AcceptProxyWebSocketRequest(destinationUri.ToWebSocketScheme());
            }
            else
            {
                var proxyService = context.RequestServices.GetRequiredService<KarrService>();

                using (var requestMessage = context.CreateProxyHttpRequest(destinationUri))
                {
                    //var prepareRequestHandler = proxyService.Options.PrepareRequest;
                    //if (prepareRequestHandler != null)
                    //{
                    //    await prepareRequestHandler(context.Request, requestMessage);
                    //}

                    using (var responseMessage = await context.SendProxyHttpRequest(requestMessage))
                    {
                        await context.CopyProxyHttpResponse(responseMessage);
                    }
                }
            }
        }
    }
}
