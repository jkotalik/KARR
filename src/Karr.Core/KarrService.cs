
using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Karr.Core
{
    public class KarrService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public KarrService(IOptions<SharedKarrOptions> options, IHttpClientFactory httpClientFactory)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _httpClientFactory = httpClientFactory;
        }

        public SharedKarrOptions Options { get; private set; }
        internal HttpClient Client => _httpClientFactory.CreateClient(); // TODO make this create a default proxy HttpClientHandler
    }
}