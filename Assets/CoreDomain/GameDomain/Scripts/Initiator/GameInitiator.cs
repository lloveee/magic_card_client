using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.GameDomain.Scripts.Mvc.Login;
using CoreDomain.GameDomain.Scripts.State.GamePlay;
using CoreDomain.GameDomain.Scripts.State.GameProfile;
using CoreDomain.Scripts.CoreInitiator;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Mvc.Loading;
using CoreDomain.Scripts.Services.DataPersistence;
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
        private readonly ILoadingController _loadingController;
        private readonly HeroCardDatabase _heroCardDatabase;
        private readonly ILogger _logger;
        
        private const string k_connection = "c_connection";

        public GameInitiator(IStateMachineService stateMachineService, ISceneInitiatorsService sceneInitiatorsService
            , GamePlayState.Factory gamePlayStateFactory, GameProfileState.Factory gameProfileStateFactory
            , ISpacetimeServer spacetimeServer, ILogger logger, ILoginController loginController, HeroCardDatabase heroCardDatabase, ILoadingController loadingController)
        {
            _spacetimeServer = spacetimeServer;
            _logger = logger;
            _loginController = loginController;
            _sceneInitiatorsService = sceneInitiatorsService;
            _stateMachineService = stateMachineService;
            _gamePlayStateFactory = gamePlayStateFactory;
            _gameProfileStateFactory = gameProfileStateFactory;
            _heroCardDatabase = heroCardDatabase;
            _loadingController = loadingController;
            _sceneInitiatorsService.RegisterInitiator(this);
        }
        
        public SceneType SceneType => SceneType.GameScene;
        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            _logger.Log("Freeze input");
            //_loginController.HideView();
            _loadingController.SetInfo("Validating data ...");
            _loadingController.SetProgress(0.5f);
            //validate data
            var tcs_data = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            await _heroCardDatabase.TryLoadRemoteData(cancellationTokenSource);
            _heroCardDatabase.TryValidateData(cancellationTokenSource, (ctx, list) => Reducer_ValidateData(ctx, list, tcs_data));
            _logger.Log("validating data...");
            try
            {
                var validated = await tcs_data.Task;
                if (!validated) return;
            }
            catch (TaskCanceledException)
            {
                _loadingController.SetInfo("Data validation Cancelled!");
                _logger.LogWarning("Data validation canceled");
                return;
            }
            _loadingController.SetInfo("Connecting player ...");
            _loadingController.SetProgress(1);
            var data = (GameInitiatorEnterData)enterData;
            
            string c_connection_sub_query = $"SELECT * FROM {k_connection} c WHERE c.Identity = '{data.LocalIdentity}'";
            
            var tcs = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            _spacetimeServer.SubscribeTableWithId(k_connection, new string[]{c_connection_sub_query}
                , (context) => OnConnectionSubApply(context, tcs)
                , (errorContext, exception) => OnConnectionSubError(errorContext, exception, tcs));
            var res = await tcs.Task;
            if (res)
            {
                await _loadingController.ShowOverlay(cancellationTokenSource);
                _loginController.ShowView();
                _loginController.TryAutoLogin();
                _loadingController.Hide();
                await _loadingController.HideOverlay(cancellationTokenSource);
            }
        }
        
        private void OnConnectionSubError(ErrorContext ctx, Exception e, TaskCompletionSource<bool> tcs)
        {
            _loadingController.SetInfo("Network error!");
            //_logger.LogWarning("Network error");
            tcs.TrySetException(e);
        }

        private void OnConnectionSubApply(SubscriptionEventContext obj, TaskCompletionSource<bool> tcs)
        {
            _logger.Log("Subscription c_connection applied");
            _logger.Log("Unfreeze input");
            _loadingController.SetInfo("");
            tcs.TrySetResult(true);
        }

        private void Reducer_ValidateData(ReducerEventContext ctx, List<HeroCard> cards, TaskCompletionSource<bool> tcs)
        {
            var e = ctx.Event;
            if (e.CallerIdentity == _spacetimeServer.LocalIdentity)
            {
                if (e.Status is Status.Failed(var error))
                {
                    _logger.Log($"data invalidate{error}");
                    tcs.TrySetResult(false);
                }
                else if (e.Status is Status.Committed)
                {
                    _logger.Log($"data validated");
                    tcs.TrySetResult(true);
                }
            }
            
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