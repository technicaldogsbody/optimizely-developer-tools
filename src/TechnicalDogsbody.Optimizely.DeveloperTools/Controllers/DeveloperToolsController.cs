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
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ClearCache;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ModelReset;
using TechnicalDogsbody.Optimizely.DeveloperTools.Framework.Requests.ResetScheduledTasks;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace TechnicalDogsbody.Optimizely.DeveloperTools.Controllers
{
    /// <summary>
    /// Controller for the developer tools
    /// </summary>
    public class DeveloperToolsController : Controller
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Controller for the developer tools
        /// </summary>
        /// <param name="mediator">Mediatr IMediator</param>
        /// <exception cref="ArgumentNullException">Exception if IMediator is null</exception>
        public DeveloperToolsController(IMediator mediator) => _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        /// <summary>
        /// Summary Page
        /// </summary>
        /// <returns>Nothing</returns>
        [Authorize(Roles = "Developers")]
        public ActionResult Index() => View();

        /// <summary>
        /// Resets versions of the ContentTypes
        /// </summary>
        /// <param name="request">Model Reset Request</param>
        /// <returns>Model Reset Response</returns>
        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<ModelResetResponse>> ModelReset(ModelResetRequest request) => View(await _mediator.Send(request));

        /// <summary>
        /// Resets the Application
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<bool>> RestartApplication(RestartApplicationRequest request) => View(await _mediator.Send(request));


        /// <summary>
        /// Clears the In Memory cache
        /// </summary>
        /// <param name="request">Cache Clear Request</param>
        /// <returns>Cache Clear Response</returns>
        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<ClearCacheResponse>> ClearCache(ClearCacheRequest request) => View(await _mediator.Send(request));

        /// <summary>
        /// Reset Scheduled Tasks
        /// </summary>
        /// <param name="request">Reset Scheduled Tasks Request</param>
        /// <returns>Reset Scheduled Tasks Response</returns>
        [Authorize(Roles = "Developers")]
        public async Task<ActionResult<ResetScheduledTasksResponse>> ResetScheduledTasks(ResetScheduledTasksRequest request) => View(await _mediator.Send(request));
    }
}
