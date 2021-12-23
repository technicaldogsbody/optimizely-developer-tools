using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ResetScheduledTasks
{
    public class ResetScheduledTasksRequestHandler : IRequestHandler<ResetScheduledTasksRequest, ResetScheduledTasksResponse>
    {
        private readonly IDeveloperToolsLogger<ResetScheduledTasksRequestHandler> _logger;
        private readonly IDbConnectionFactory _connectionFactory;

        private const string Query = "SELECT [pkID] as Id,[Name],[IsRunning] FROM [tblScheduledItem] ORDER BY [Name]";
        private const string Update = "Update [tblScheduledItem] Set [IsRunning] = 0 WHERE [pkId] = '{0}'";

        public ResetScheduledTasksRequestHandler(IDeveloperToolsLogger<ResetScheduledTasksRequestHandler> logger, IDbConnectionFactory connectionFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public async Task<ResetScheduledTasksResponse> Handle(ResetScheduledTasksRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug(nameof(Handle), "Start - Request: {request}, CancellationToken: {cancellationToken}", request, cancellationToken);
            var sw = Stopwatch.StartNew();

            var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<ScheduledTask>(Query);

            if (request.Reset && request.Ids != null && request.Ids.Any())
            {
                foreach (var record in result.Where(x => request.Ids.Contains(x.Id)))
                {
                    await db.ExecuteAsync(string.Format(Update, record.Id));
                }

                result = await db.QueryAsync<ScheduledTask>(Query);
            }

            var response = new ResetScheduledTasksResponse
            {
                Reset = request.Reset,
                Tasks = result
            };

            _logger.LogDebug(nameof(Handle),
                "Completed - Time: {sw.ElapsedMilliseconds}ms, Response: {response}", sw.ElapsedMilliseconds, response);

            return response;
        }
    }
}
