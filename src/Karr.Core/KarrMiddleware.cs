﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Karr.Core
{
    //public class IKarrBuilder 
    //{
    //    public ICollection<MatcherEndpoint> Endpoints {get;} = new List<MatcherEndpoint>();
        
    //    public IKarrBuilder AddEndpoint(string template, string matchUri, params object[] metadata)
    //    {
    //        // TODO transform template and match uri
    //        Endpoints.Add(new MatcherEndpoint(template, matchUri, metadata));
    //        return this;
    //    }
    //}

    //internal class KarrEndpointDataSource : EndpointDataSource
    //{
    //    // override 
    //    public KarrEndpointDataSource(IKarrBuilder builder)
    //    {

    //    }
    //}
}
