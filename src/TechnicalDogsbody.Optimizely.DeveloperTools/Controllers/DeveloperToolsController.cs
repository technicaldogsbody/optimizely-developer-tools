using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Dapper;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.Licensing.RestrictionTypes;
using EPiServer.Shell.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using TechnicalDogsbody.Optimizely.DeveloperTools.Contracts;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Controllers
{
    public class DeveloperToolsController : Controller
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IContentLoader _contentLoader;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ISynchronizedObjectInstanceCache _synchronizedObjectInstanceCache;
        private readonly IMemoryCache _memoryCache;

        public DeveloperToolsController(IHostApplicationLifetime applicationLifetime,
            IContentLoader contentLoader, IDbConnectionFactory connectionFactory,
            ISynchronizedObjectInstanceCache synchronizedObjectInstanceCache, IMemoryCache memoryCache)
        {
            _applicationLifetime = applicationLifetime ?? throw new ArgumentNullException(nameof(applicationLifetime));
            _contentLoader = contentLoader ?? throw new ArgumentNullException(nameof(contentLoader));
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            _synchronizedObjectInstanceCache = synchronizedObjectInstanceCache ?? throw new ArgumentNullException(nameof(synchronizedObjectInstanceCache));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [Authorize(Roles = "Developers")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<(string, IEnumerable<(int, string, string)>)>> ModelReset(bool reset, IEnumerable<int> ids)
        {
            var startPage = _contentLoader.Get<IContentData>(ContentReference.StartPage);
            var startPageType = startPage.GetType();

            var version = System.Diagnostics.FileVersionInfo.GetVersionInfo(startPageType.BaseType?.Assembly.Location);
            var query =
                "SELECT [pkId], [Name], [ModelType] FROM [tblContentType] WHERE PATINDEX('%Version=%.%', [ModelType]) > 1";
            var db = _connectionFactory.CreateConnection();
            var result = await db.QueryAsync<(int, string, string)>(query);

            if (reset)
            {
                foreach (var record in result.Where(x => ids.Contains(x.Item1)))
                {
                    var currentVersion = Regex.Match(record.Item3, @"Version=\d+.\d+.\d+.\d+");
                    await db.ExecuteAsync(
                        $"Update [tblContentType] Set [ModelType] = REPLACE(ModelType, '{currentVersion}', 'Version={version.FileVersion}') WHERE [pkId] = {record.Item1}");
                }
            }

            result = await db.QueryAsync<(int, string, string)>(query);

            return View((!reset, version.FileVersion, result));
        }

        [Authorize(Roles = "Developers")]
        public ActionResult<bool> RestartApplication(bool restart)
        {
            if (restart)
            {
                _applicationLifetime.StopApplication();

                return View(true);
            }
            else
            {
                return View(false);
            }
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
