using System.Threading;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.StateMachine;
using UnityEngine;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.Scripts.State.GamePractice
{
    public class GamePracticeState: BaseGameState<GamePracticeInitiatorEnterData>
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        
        public GamePracticeState(ISceneLoaderService sceneLoaderService, GamePracticeInitiatorEnterData enterData, ILogger logger) : base(enterData, logger)
        {
            _sceneLoaderService = sceneLoaderService;
        }
        
        public override GameStateType GameStateType => GameStateType.Practice;
        
        public override async Awaitable LoadState(CancellationTokenSource cancellationTokenSource)
        {
            await base.LoadState(cancellationTokenSource);
            await _sceneLoaderService.TryLoadScene(SceneType.GamePracticeScene, EnterData, cancellationTokenSource);
        }
        
        public override async Awaitable StartState(CancellationTokenSource cancellationTokenSource)
        {
            await base.StartState(cancellationTokenSource);
            await _sceneLoaderService.StartScene(SceneType.GamePracticeScene, EnterData, cancellationTokenSource);
        }
        
        public override async Awaitable ExitState(CancellationTokenSource cancellationTokenSource)
        {
            await base.ExitState(cancellationTokenSource);
            await _sceneLoaderService.TryUnloadScene(SceneType.GamePracticeScene, cancellationTokenSource);
        }
        
        public class Factory : PlaceholderFactory<GamePracticeInitiatorEnterData, GamePracticeState>
        {
            
        }
    }
}