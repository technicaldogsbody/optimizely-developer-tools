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
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ModelReset
{
    public class ModelResetRequestHandler : IRequestHandler<ModelResetRequest, ModelResetResponse>
    {
        private readonly IDeveloperToolsLogger<ModelResetRequestHandler> _logger;
        private readonly IContentLoader _contentLoader;
        private readonly IDbConnectionFactory _connectionFactory;

        const string Query = "SELECT [pkId] As Id, [Name], [ModelType] FROM [tblContentType] WHERE PATINDEX('%Version=%.%', [ModelType]) > 1";

        private const string Update =
            "Update [tblContentType] Set [ModelType] = REPLACE(ModelType, '{0}', 'Version={1}') WHERE [pkId] = {2}";


        public ModelResetRequestHandler(
            IDeveloperToolsLogger<ModelResetRequestHandler> logger, 
            IContentLoader contentLoader,
            IDbConnectionFactory connectionFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _contentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<ModelResetResponse> Handle(ModelResetRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug(nameof(Handle), "Start - Request: {request}, CancellationToken: {cancellationToken}", request, cancellationToken);
            
            var sw = Stopwatch.StartNew();

            var startPage = _contentLoader.Get<IContentData>(ContentReference.StartPage);
            var startPageType = startPage.GetType();

            if (startPageType.BaseType?.Assembly.Location != null)
            {
                var version =
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(startPageType.BaseType?.Assembly.Location);

                var db = _connectionFactory.CreateConnection();
                var result = await db.QueryAsync<ContentType>(Query);

                if (request.Reset)
                {
                    foreach (var record in result.Where(x => request.Ids.Contains(x.Id)))
                    {
                        var currentVersion = Regex.Match(record.ModelType, @"Version=\d+.\d+.\d+.\d+");
                        await db.ExecuteAsync(string.Format(Update, currentVersion, version.FileVersion, record.Id));
                    }
                }

                result = await db.QueryAsync<ContentType>(Query);

                var response = new ModelResetResponse
                    { Reset = request.Reset, Version = version.FileVersion, ContentTypes = result };

                _logger.LogDebug(nameof(Handle),
                    "Completed - Time: {sw.ElapsedMilliseconds}ms, Response: {response}", sw.ElapsedMilliseconds, response);

                return response;
            }
            else
            {
                _logger.LogError(nameof(Handle), 
                    "Start page type unknown - Request: {request}, StartPage: {startPage}, StartPageType: {startPageType}, Time: {sw.ElapsedMilliseconds}ms",
                    request, startPage, startPageType, sw.ElapsedMilliseconds);
                throw new ApplicationException("Start page type unknown");
            }
        }
    }
}
