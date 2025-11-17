using System;
using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Services.Database;
using CoreDomain.GameDomain.Scripts.State.GamePractice;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Initiator
{
    public class GamePracticeInitiator : IGamePracticeInitiator, ISceneInitiator
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;
        private readonly GamePracticeDatabase _database;
        private readonly ILogger _logger;

        public GamePracticeInitiator(ICommandFactory commandFactory, ISceneInitiatorsService sceneInitiatorsService
            , ILogger logger, GamePracticeDatabase database)
        {
            _commandFactory = commandFactory;
            _sceneInitiatorsService = sceneInitiatorsService;
            _logger = logger;
            _database = database;
            _sceneInitiatorsService.RegisterInitiator(this);
        }

        public SceneType SceneType => SceneType.GamePracticeScene;
        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GamePracticeInitiatorEnterData)enterData;
            _logger.Log("Try Sub Practice Match Context");
            try
            {
                var res = await _database.SubRemoteServer(data.Username, cancellationTokenSource);
                if (res == false) _logger.LogError("Failed to load match context");
                else
                {
                    _logger.Log("Successfully loaded match context");
                    _logger.Log($"{_database.Db.SelfContext}");
                    _logger.Log($"{_database.Db.MatchContext}");
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
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