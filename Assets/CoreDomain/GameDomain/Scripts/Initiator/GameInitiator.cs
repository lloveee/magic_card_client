using System;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.GameDomain.Scripts.Mvc.Login;
using CoreDomain.GameDomain.Scripts.State.GamePlayState;
using CoreDomain.GameDomain.Scripts.State.GameProfileState;
using CoreDomain.Scripts.CoreInitiator;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Services.StateMachine;
using CoreDomain.Scripts.Utils;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.Scripts.Initiator
{
    public class GameInitiator : ISceneInitiator, IGameInitiator
    {
        private readonly IStateMachineService _stateMachineService;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;
        private readonly GamePlayState.Factory _gamePlayStateFactory;
        private readonly GameProfileState.Factory _gameProfileStateFactory;
        private readonly ISpacetimeServer _spacetimeServer;
        private readonly ILoginController _loginController;
        private readonly ILogger _logger;
        
        private const string k_connection = "c_connection";

        public GameInitiator(IStateMachineService stateMachineService, ISceneInitiatorsService sceneInitiatorsService
            , GamePlayState.Factory gamePlayStateFactory, GameProfileState.Factory gameProfileStateFactory
            , ISpacetimeServer spacetimeServer, ILogger logger, ILoginController loginController)
        {
            _spacetimeServer = spacetimeServer;
            _logger = logger;
            _loginController = loginController;
            _sceneInitiatorsService = sceneInitiatorsService;
            _stateMachineService = stateMachineService;
            _gamePlayStateFactory = gamePlayStateFactory;
            _gameProfileStateFactory = gameProfileStateFactory;
            _sceneInitiatorsService.RegisterInitiator(this);
        }
        
        public SceneType SceneType => SceneType.GameScene;
        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GameInitiatorEnterData)enterData;
            _logger.Log("Freeze input");
            _loginController.HideView();
            string c_connection_sub_query = $"SELECT * FROM {k_connection} c WHERE c.Identity = '{data.LocalIdentity}'";
            
            var tcs = new TaskCompletionSource<bool>();
            
            cancellationTokenSource.Token.Register(() => tcs.TrySetCanceled());
            _spacetimeServer.SubscribeTableWithId(k_connection, new string[]{c_connection_sub_query}
                , (context) => OnConnectionSubApply(context, tcs)
                , (errorContext, exception) => OnConnectionSubError(errorContext, exception, tcs));
            
            await tcs.Task;
        }
        
        private void OnConnectionSubError(ErrorContext ctx, Exception e, TaskCompletionSource<bool> tcs)
        {
            _logger.LogWarning("Network error");
            tcs.TrySetException(e);
            //_loginController.UnfreezeInterface();
        }

        private void OnConnectionSubApply(SubscriptionEventContext obj, TaskCompletionSource<bool> tcs)
        {
            _logger.Log("Subscription c_connection applied");
            _logger.Log("Unfreeze input");
            tcs.TrySetResult(true);
            _loginController.ShowView();
        }

        public Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            return AwaitableUtils.CompletedTask;
        }

        public Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            _sceneInitiatorsService.UnregisterInitiator(this);
            return AwaitableUtils.CompletedTask;
        }
    }
}