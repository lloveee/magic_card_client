using System.Threading;
using CoreDomain.GameDomain.Scripts.State.GamePractice;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Initiator
{
    public class GamePracticeInitiator : IGamePracticeInitiator, ISceneInitiator
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;

        public GamePracticeInitiator(ICommandFactory commandFactory, ISceneInitiatorsService sceneInitiatorsService)
        {
            _commandFactory = commandFactory;
            _sceneInitiatorsService = sceneInitiatorsService;
            _sceneInitiatorsService.RegisterInitiator(this);
        }

        public SceneType SceneType => SceneType.GamePracticeScene;
        public async Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GamePracticeInitiatorEnterData)enterData;
            
            await AwaitableUtils.CompletedTask;
        }

        public async Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GamePracticeInitiatorEnterData)enterData;
            await AwaitableUtils.CompletedTask;
        }

        public async Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            _sceneInitiatorsService.UnregisterInitiator(this);
            await AwaitableUtils.CompletedTask;
        }
    }
}