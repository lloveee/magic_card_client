using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.ManagementDomain.Scripts.Initiator
{
    public class ManagementInitiator : ISceneInitiator, IManagementInitiator
    {
        public SceneType SceneType => SceneType.ManagementScene;
        private readonly ILogger _logger;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;
        //Test
        private readonly HeroCardDatabase _heroCardDatabase;
        
        public ManagementInitiator(ILogger logger, ISceneInitiatorsService sceneInitiatorsService, HeroCardDatabase heroCardDatabase)
        {
            _logger = logger;
            _sceneInitiatorsService = sceneInitiatorsService;
            _sceneInitiatorsService.RegisterInitiator(this);
            _heroCardDatabase = heroCardDatabase;
        }
        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            // Start Finish
            _heroCardDatabase.TryConnectSpacetimeDb();
            await AwaitableUtils.CompletedTask;
        }

        public async Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            await AwaitableUtils.CompletedTask;
        }

        public async Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            _sceneInitiatorsService.UnregisterInitiator(this);
            await AwaitableUtils.CompletedTask;
        }
    }
}