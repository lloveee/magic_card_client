using System.Threading;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.StateMachine;
using UnityEngine;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.Scripts.State.GameProfileState
{
    public class GameProfileState : BaseGameState<GameProfileInitiatorEnterData>
    {
        private readonly ISceneLoaderService _sceneLoaderService;
        
        public GameProfileState(ISceneLoaderService sceneLoaderService, GameProfileInitiatorEnterData enterData, ILogger logger) : base(enterData, logger)
        {
            _sceneLoaderService = sceneLoaderService;
        }

        public override GameStateType GameStateType => GameStateType.Profile;
        public override async Awaitable LoadState(CancellationTokenSource cancellationTokenSource)
        {
            await base.LoadState(cancellationTokenSource);
            await _sceneLoaderService.TryLoadScene(SceneType.GameProfileScene, EnterData, cancellationTokenSource);
        }
        
        public override async Awaitable StartState(CancellationTokenSource cancellationTokenSource)
        {
            await base.StartState(cancellationTokenSource);
            await _sceneLoaderService.StartScene(SceneType.GameProfileScene, EnterData, cancellationTokenSource);
        }
        
        public override async Awaitable ExitState(CancellationTokenSource cancellationTokenSource)
        {
            await base.ExitState(cancellationTokenSource);
            await _sceneLoaderService.TryUnloadScene(SceneType.GameProfileScene, cancellationTokenSource);
        }
        
        public class Factory : PlaceholderFactory<GameProfileInitiatorEnterData, GameProfileState>
        {
            
        }
    }
}