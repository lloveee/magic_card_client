using System.Threading;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.StateMachine;
using UnityEngine;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.Scripts.State.GamePlayState
{
    public class GamePlayState : BaseGameState<GamePlayInitiatorEnterData>
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        
        public GamePlayState(ISceneLoaderService sceneLoaderService, GamePlayInitiatorEnterData enterData, ILogger logger) : base(enterData, logger)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public override GameStateType GameStateType => GameStateType.GamePlay;

        public override async Awaitable LoadState(CancellationTokenSource cancellationTokenSource)
        {
            await base.LoadState(cancellationTokenSource);
            await _sceneLoaderService.TryLoadScene(SceneType.GamePlayScene, EnterData, cancellationTokenSource);
        }

        public override async Awaitable StartState(CancellationTokenSource cancellationTokenSource)
        {
            await base.StartState(cancellationTokenSource);
            await _sceneLoaderService.StartScene(SceneType.GamePlayScene, EnterData, cancellationTokenSource);
        }
        
        public override async Awaitable ExitState(CancellationTokenSource cancellationTokenSource)
        {
            await base.ExitState(cancellationTokenSource);
            await _sceneLoaderService.TryUnloadScene(SceneType.GamePlayScene, cancellationTokenSource);
        }

        public class Factory : PlaceholderFactory<GamePlayInitiatorEnterData, GamePlayState>
        {
            
        }
    }
}