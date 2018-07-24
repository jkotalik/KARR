using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Routing;

namespace Karr.Core
{
    public class DefaultEndpointDataSourceBuilder : IEndpointDataSourceBuilder
    {
        public IList<Endpoint> Endpoints { get; } = new List<Endpoint>();
    }
}
