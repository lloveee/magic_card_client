using System.Threading;
using CoreDomain.GameDomain.Scripts.State.GamePlayState;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Initiator
{
    public class GamePlayInitiator : ISceneInitiator, IGamePlayInitiator
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;

        public GamePlayInitiator(ICommandFactory commandFactory, ISceneInitiatorsService sceneInitiatorsService)
        {
            _commandFactory = commandFactory;
            _sceneInitiatorsService = sceneInitiatorsService;
            _sceneInitiatorsService.RegisterInitiator(this);
        }
        public SceneType SceneType => SceneType.GamePlayScene;
        public Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GamePlayInitiatorEnterData)enterData;
            return AwaitableUtils.CompletedTask;
        }

        public Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GamePlayInitiatorEnterData)enterData;
            return AwaitableUtils.CompletedTask;
        }

        public Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            _sceneInitiatorsService.UnregisterInitiator(this);
            return AwaitableUtils.CompletedTask;
        }
    }
}