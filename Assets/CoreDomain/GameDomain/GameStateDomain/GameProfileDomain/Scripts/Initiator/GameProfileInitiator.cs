using System;
using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;
using CoreDomain.GameDomain.Scripts.State.GameProfileState;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Initiator
{
    public class GameProfileInitiator: IGameProfileInitiator, ISceneInitiator
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;
        private readonly IHomeController _homeController;
        private readonly GameProfileDatabase _gameProfileDatabase;
        private readonly ILogger _logger;

        public GameProfileInitiator(ICommandFactory commandFactory, ISceneInitiatorsService sceneInitiatorsService, IHomeController homeController, 
            GameProfileDatabase gameProfileDatabase, ILogger logger)
        {
            _commandFactory = commandFactory;
            _homeController = homeController;
            _sceneInitiatorsService = sceneInitiatorsService;
            _gameProfileDatabase = gameProfileDatabase;
            _logger = logger;
            _sceneInitiatorsService.RegisterInitiator(this);
        }
        public SceneType SceneType => SceneType.GameProfileScene;
        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GameProfileInitiatorEnterData)enterData;
            _logger.Log("Try Load Profile Entry");
            try
            {
                var res = await _gameProfileDatabase.SubRemoteServer(data.CurrentPlayer.Username, cancellationTokenSource);
                if (res == false)
                {
                    _logger.LogError("Failed to load game profile");
                }
                else
                {
                    _homeController.InitHomeData(_gameProfileDatabase.Db.CurrentPlayer);
                    _logger.Log($"{_gameProfileDatabase.Db.CurrentPlayer.Nickname}");
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
            await AwaitableUtils.CompletedTask;
        }

        public Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            return AwaitableUtils.CompletedTask;
        }

        public async Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            await _gameProfileDatabase.UnsubRemoteServer(cancellationTokenSource);
            _logger.Log("Unsub... Profile Exit");
            _sceneInitiatorsService.UnregisterInitiator(this);
            await AwaitableUtils.CompletedTask;
        }
    }
}