using System.Threading;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Utils;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Services.StateMachine
{
    public abstract class BaseGameState<T> : IGameState where T : class, IInitiatorEnterData
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private ILogger _logger;
        public T EnterData { get; }

        protected BaseGameState(T enterData, ILogger logger)
        {
            EnterData = enterData;
            _logger = logger;
            _cancellationTokenSource = new CancellationTokenSource();
        }
        public CancellationTokenSource CancellationTokenSource 
            => CancellationTokenSource.CreateLinkedTokenSource(_cancellationTokenSource.Token);
        public abstract GameStateType GameStateType { get; }
        public virtual Awaitable LoadState(CancellationTokenSource cancellationTokenSource)
        {
            _logger.Log($"Load State: [{GameStateType}]");
            return AwaitableUtils.CompletedTask;
        }

        public virtual Awaitable StartState(CancellationTokenSource cancellationTokenSource)
        {
            _logger.Log($"Start State: [{GameStateType}]");
            return AwaitableUtils.CompletedTask;
        }

        public virtual Awaitable ExitState(CancellationTokenSource cancellationTokenSource)
        {
            _logger.Log($"Exit State: [{GameStateType}]");
            _cancellationTokenSource.Cancel();
            return AwaitableUtils.CompletedTask;
        }
    }
}