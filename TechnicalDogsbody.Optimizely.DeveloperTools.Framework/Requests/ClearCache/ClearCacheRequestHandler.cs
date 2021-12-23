using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using TechnicalDogsbody.Optimizely.DeveloperTools.Controllers;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ClearCache
{
    public class ClearCacheRequestHandler : IRequestHandler<ClearCacheRequest, ClearCacheResponse>
    {
        private readonly IDeveloperToolsLogger<ClearCacheRequestHandler> _logger;
        private readonly IMemoryCache _memoryCache;

        public ClearCacheRequestHandler(IDeveloperToolsLogger<ClearCacheRequestHandler> logger, IMemoryCache memoryCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<ClearCacheResponse> Handle(ClearCacheRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug(nameof(Handle), "Start - Request: {request}, CancellationToken: {cancellationToken}", request, cancellationToken);
            var sw = Stopwatch.StartNew();

            var cachedEntries = _memoryCache.GetKeys().Cast<string>().OrderBy(x => x).ToList();

            if (request.Reset)
            {
                foreach (var entry in cachedEntries)
                {
                    _memoryCache.Remove(entry.ToString());
                }

                cachedEntries = _memoryCache.GetKeys().Cast<string>().OrderBy(x => x).ToList();
            }

            var response = await Task.FromResult(new ClearCacheResponse {Reset = request.Reset, Keys = cachedEntries});

            _logger.LogDebug(nameof(Handle),
                "Completed - Time: {sw.ElapsedMilliseconds}ms, Response: {response}", sw.ElapsedMilliseconds, response);

            return response;
        }
    }
}
