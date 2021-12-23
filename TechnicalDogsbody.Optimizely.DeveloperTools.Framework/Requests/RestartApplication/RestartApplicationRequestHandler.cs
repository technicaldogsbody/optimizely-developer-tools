using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using EPiServer;
using EPiServer.Core;
using MediatR;
using Microsoft.Extensions.Hosting;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ModelReset
{
    public class RestartApplicationRequestHandler : IRequestHandler<RestartApplicationRequest, bool>
    {
        private readonly IDeveloperToolsLogger<RestartApplicationRequestHandler> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;


        public RestartApplicationRequestHandler(
            IDeveloperToolsLogger<RestartApplicationRequestHandler> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
        }

        public async Task<bool> Handle(RestartApplicationRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug(nameof(Handle), "Start - Request: {request}, CancellationToken: {cancellationToken}", request, cancellationToken);
            
            var sw = Stopwatch.StartNew();

            if (request.Restart)
            {
                _applicationLifetime.StopApplication();

                _logger.LogDebug(nameof(Handle),
                    "Completed - Time: {sw.ElapsedMilliseconds}ms, Response: true", sw.ElapsedMilliseconds);

                return await Task.FromResult(true);
            }
            else
            {
                _logger.LogDebug(nameof(Handle),
                    "Completed - Time: {sw.ElapsedMilliseconds}ms, Response: false", sw.ElapsedMilliseconds);

                return await Task.FromResult(false);
            }
        }
    }
}
