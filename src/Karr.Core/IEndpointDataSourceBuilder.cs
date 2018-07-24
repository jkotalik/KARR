using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Patterns;

namespace Karr.Core
{
    public interface IEndpointDataSourceBuilder
    {
        IList<Endpoint> Endpoints { get; }
    }

    public static class IEndpointDataSourceBuilderExtensions
    {
        // TODO wrap these into configuration via json or something better 
        public static IEndpointDataSourceBuilder AddProxyEndpoint(this IEndpointDataSourceBuilder builder,
            RoutePattern routePattern,
            RouteValueDictionary requiredValues,
            int order,
            EndpointMetadataCollection metadata,
            string displayName,
            Uri matchUri)
        {
            builder.Endpoints.Add(new ProxyEndpoint(routePattern, requiredValues, order, metadata, displayName, matchUri));
            return builder;
        }
    }
}
