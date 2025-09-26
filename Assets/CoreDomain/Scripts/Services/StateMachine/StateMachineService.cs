using System;
using System.Threading;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Services.StateMachine
{
    public class StateMachineService : IStateMachineService
    {
        public IGameState CurrentState => _currentState;
        private IGameState _currentState = null;
        private ILogger _logger;

        public StateMachineService(ILogger logger)
        {
            _logger = logger;
        }
        public async Awaitable EnterInitialState(IGameState initialState, CancellationTokenSource cancellationTokenSource)
        {
            _currentState = initialState;
            await _currentState.LoadState(cancellationTokenSource);
            await _currentState.StartState(cancellationTokenSource);
        }

        public void SwitchState(IGameState nextState)
        {
            _ = SwitchStateAsync(nextState);
        }

        public async Awaitable SwitchStateAsync(IGameState nextState)
        {
            try
            {
                var cancellationTokenSource =
                    CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);

                if (_currentState == null)
                {
                    _logger.LogError("No state to switch from, need to initialize a game state first!");
                    return;
                }

                await _currentState.ExitState(cancellationTokenSource);
                _currentState = nextState;
                await _currentState.LoadState(cancellationTokenSource);
                await _currentState.StartState(cancellationTokenSource);
                
            }
            catch (OperationCanceledException)
            {
                _logger.Log("Switching state operation was cancelled");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}