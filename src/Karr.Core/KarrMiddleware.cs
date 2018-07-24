using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Karr.Core
{
    // configure
    public class KarrMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly KarrOptions _options;
        public KarrMiddleware(RequestDelegate next, IOptions<KarrOptions> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // TODO make this validation earlier?
            if (options.Value.Scheme == null)
            {
                throw new ArgumentException("Options parameter must specify scheme.", nameof(options));
            }
            if (!options.Value.Host.HasValue)
            {
                throw new ArgumentException("Options parameter must specify host.", nameof(options));
            }
            _next = next;
            _options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            var feature = context.Features.Get<IEndpointFeature>();
            if (feature == null)
            {
                throw new InvalidOperationException("GlobalRoutingMiddleware wasn't run.");
            }

            // Generate URL, other middleware have already been executed.
            // At this point, we need to transform the uri based on routing rules
            // TODO perf and use routing rules correctly.
            var uri = new Uri(UriHelper.BuildAbsolute(_options.Scheme, _options.Host, new PathString(), context.Request.Path, context.Request.QueryString));

            return context.ProxyRequest(uri);
        }
    }

    public class IKarrBuilder 
    {
        public ICollection<MatcherEndpoint> Endpoints {get;} = new List<MatcherEndpoint>();
        
        public IKarrBuilder AddEndpoint(string template, string matchUri, params object[] metadata)
        {
            // TODO transform template and match uri
            Endpoints.Add(new MatcherEndpoint(template, matchUri, metadata));
            return this;
        }
    }

    internal class KarrEndpointDataSource : EndpointDataSource
    {
        // override 
        public KarrEndpointDataSource(IKarrBuilder builder)
        {

        }

        public 
    }
}
