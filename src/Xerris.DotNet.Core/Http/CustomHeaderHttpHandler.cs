using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Xerris.DotNet.Core.Http
{
    public class CustomHeaderHttpHandler : DelegatingHandler
    {
        private readonly IHeaderProvider headerProvider;

        public CustomHeaderHttpHandler(IHeaderProvider headerProvider)
        {
            this.headerProvider = headerProvider;
        }

        #region Overrides of DelegatingHandler

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var (name, value) = await headerProvider.GetHeaderAsync();

            request.Headers.Add(name, value);

            return await base.SendAsync(request, cancellationToken);
        }

        #endregion
    }
}