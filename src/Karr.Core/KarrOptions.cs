using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Karr.Core
{
    public class KarrOptions
    {
        // List of endpoints to route to 
        // TODO how does dispatcher deal with multiple end points
        /// <summary>
        /// Destination uri scheme
        /// </summary>
        public string Scheme { get; set; }
        /// <summary>
        /// Destination uri host
        /// </summary>
        public HostString Host { get; set; }
    }
}
