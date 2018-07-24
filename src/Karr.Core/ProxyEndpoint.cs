using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Karr.Core
{
    public class ProxyEndpoint : Endpoint
    {
        public ProxyEndpoint(
            RoutePattern routePattern,
            RouteValueDictionary requiredValues,
            int order,
            EndpointMetadataCollection metadata,
            string displayName,
            Uri matchUri)
            : base(metadata, displayName)
        {
            Invoker = (next) => (httpContext) =>
                {
                    return httpContext.ProxyRequest(matchUri);
                };
            
            RoutePattern = routePattern ?? throw new ArgumentNullException(nameof(routePattern));
            RequiredValues = requiredValues;
            Order = order;
            MatchUri = matchUri;
        }

        public Func<RequestDelegate, RequestDelegate> Invoker { get; }

        public int Order { get; }

        // Values required by an endpoint for it to be successfully matched on link generation
        public IReadOnlyDictionary<string, object> RequiredValues { get; }

        public RoutePattern RoutePattern { get; }

        public Uri MatchUri { get; }
    }
}
