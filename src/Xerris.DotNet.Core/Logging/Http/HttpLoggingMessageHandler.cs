using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Xerris.DotNet.Core.Logging.Http
{
    public class HttpLoggingMessageHandler : DelegatingHandler
    {
        public HttpLoggingMessageHandler(HttpMessageHandler innerHandler = null)
            : base(innerHandler ?? new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var req = request;
            var id = Guid.NewGuid().ToString();
            var msg = $"[{id} - Request]";

            Log.Information("{Message}========Start==========", msg);
            Log.Debug("{Message} {Method} {PathAndQuery} {Scheme}/{Version}",
                msg, req.Method, req.RequestUri.PathAndQuery, req.RequestUri.Scheme, req.Version);
            Log.Information("{Message} Host: {Scheme}://{Host}",
                msg, req.RequestUri.Scheme, req.RequestUri.Host);

            foreach (var (key, value) in req.Content.Headers.Where(h 
                         => string.Equals(h.Key, "Authorization",StringComparison.CurrentCultureIgnoreCase)))
            {
                Log.Debug("{Message} {Key}: {Value}", msg, key, string.Join(", ", value));
            }
            
            if (req.Content != null)
            {
                foreach (var (key, value) in req.Content.Headers)
                    Log.Debug("{Message} {Key}: {Value}", msg, key, string.Join(", ", value));

                if (req.Content is StringContent || IsTextBasedContentType(req.Headers) ||
                    IsTextBasedContentType(req.Content.Headers))
                {
                    var result = await req.Content.ReadAsStringAsync();

                    Log.Information("{Message} Content:", msg);
                    Log.Debug("{Message} {@Result}", msg, result);
                }
            }

            var start = DateTime.Now;

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            var end = DateTime.Now;

            Log.Debug("{Message} {Duration}", msg, end - start);
            Log.Debug("{Message}==========End==========", msg);

            msg = $"[{id} - Response]";
            Log.Debug("{Message}=========Start=========", msg);

            var resp = response;

            Log.Debug("{Message} {Scheme}/{Version} {StatusCode} {ReasonPhrase}",
                msg, req.RequestUri.Scheme.ToUpper(), resp.Version, (int) resp.StatusCode, resp.ReasonPhrase);

            foreach (var (key, value) in resp.Headers)
                Log.Debug("{Message} {key}: {Value}", msg, key, string.Join(", ", value));

            if (resp.Content != null)
            {
                foreach (var (key, value) in resp.Content.Headers)
                    Log.Debug($"{msg} {key}: {string.Join(", ", value)}");

                if (resp.Content is StringContent || this.IsTextBasedContentType(resp.Headers) ||
                    IsTextBasedContentType(resp.Content.Headers))
                {
                    start = DateTime.Now;
                    var result = await resp.Content.ReadAsStringAsync();
                    end = DateTime.Now;

                    Log.Debug("{Message} Content:", msg);
                    Log.Debug("{Message} {@Result}", msg, result);
                    Log.Debug("{Duration}", end - start);
                }
            }

            Log.Debug("{Message}==========End==========", msg);
            return response;
        }

        private readonly string[] types = {"html", "text", "xml", "json", "txt", "x-www-form-urlencoded"};

        private bool IsTextBasedContentType(HttpHeaders headers)
        {
            if (!headers.TryGetValues("Content-Type", out var values))
                return false;
            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}