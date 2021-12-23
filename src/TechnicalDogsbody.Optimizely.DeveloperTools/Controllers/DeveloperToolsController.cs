using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.Shell.Navigation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using TechnicalDogsbody.Optimizely.DeveloperTools.Core.Contracts;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ModelReset;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Controllers
{
    public class DeveloperToolsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IMemoryCache _memoryCache;

        public DeveloperToolsController(IMediator mediator, IDbConnectionFactory connectionFactory, IMemoryCache memoryCache)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [Authorize(Roles = "Developers")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<ModelResetResponse>> ModelReset(bool reset, IEnumerable<int> ids)
        {
            var response = await _mediator.Send(new ModelResetRequest { Reset = reset, Ids = ids });

            return View(response);
        }

        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<bool>> RestartApplication(bool restart)
        {
            return await _mediator.Send(new RestartApplicationRequest { Restart = restart });
        }

        [Authorize(Roles = "Developers")]
        public ActionResult<List<string>> ClearCache(bool reset)
        {
            var cachedEntries = _memoryCache.GetKeys().Cast<string>().OrderBy(x => x).ToList();

            if (reset)
            {
                foreach (var entry in cachedEntries)
                {
                    _memoryCache.Remove(entry.ToString());
                }
            }

            return View((reset, cachedEntries));
        }

        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<IEnumerable<(Guid, string, bool)>>> ResetScheduledTasks(bool reset, IEnumerable<Guid> ids)
        {
            var query =
                "SELECT [pkID],[Name],[IsRunning] FROM [tblScheduledItem] ORDER BY [Name]";
            var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<(Guid, string, bool)>(query);

            if (reset)
            {
                foreach (var record in result.Where(x => ids.Contains(x.Item1)))
                {
                    await db.ExecuteAsync(
                        $"Update [tblScheduledItem] Set [IsRunning] = 0 WHERE [pkId] = '{record.Item1}'");
                }
            }

            result = await db.QueryAsync<(Guid, string, bool)>(query);

            return View((reset, result));
        }
    }
}
