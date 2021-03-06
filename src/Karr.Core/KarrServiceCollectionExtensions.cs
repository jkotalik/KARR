﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Karr.Core
{
    public static class KarrServiceCollectionExtensions
    {
        public static IServiceCollection AddKarr(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddHttpClient();
            services.AddSingleton<IEndpointDataSourceBuilder>(new DefaultEndpointDataSourceBuilder());

            return services.AddSingleton<KarrService>();
        }
    }
}
