using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Karr.Core
{
    public static class KarrExtensions
    {
        public static void UseKarr(this IApplicationBuilder app, string baseUri)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            var uri = new Uri(baseUri);

            var options = new KarrOptions
            {
                Scheme = uri.Scheme,
                Host = new HostString(uri.Authority)
            };

            app.UseMiddleware<KarrMiddleware>(options);
        }
        public static void UseKarr(this IApplicationBuilder app, Uri baseUri)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (baseUri == null)
            {
                throw new ArgumentNullException(nameof(baseUri));
            }

            var options = new KarrOptions
            {
                Scheme = baseUri.Scheme,
                Host = new HostString(baseUri.Authority)
            };

            app.UseMiddleware<KarrMiddleware>(options);
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
