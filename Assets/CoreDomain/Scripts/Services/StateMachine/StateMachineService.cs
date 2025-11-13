using System;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.Scripts.Mvc.Loading;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Services.StateMachine
{
    public class StateMachineService : IStateMachineService
    {
        public IGameState CurrentState => _currentState;
        private IGameState _currentState = null;
        private ILogger _logger;
        private readonly ILoadingController _loadingController;

        public StateMachineService(ILogger logger, ILoadingController loadingController)
        {
            _logger = logger;
            _loadingController = loadingController;
        }
        public async Awaitable EnterInitialState(IGameState initialState, CancellationTokenSource cancellationTokenSource)
        {
            _currentState = initialState;
            _loadingController.SetProgress(0.5f);
            await _currentState.LoadState(cancellationTokenSource);
            await _currentState.StartState(cancellationTokenSource);
            _loadingController.SetProgress(1f);
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

                await _loadingController.ShowOverlay(cancellationTokenSource);
                _loadingController.Show();
                _loadingController.SetProgress(0.5f);
                await _loadingController.HideOverlay(cancellationTokenSource);
                await _currentState.ExitState(cancellationTokenSource);
                
                _currentState = nextState;
                await _currentState.LoadState(cancellationTokenSource);
                _loadingController.SetProgress(1);
                await _currentState.StartState(cancellationTokenSource);
                await _loadingController.ShowOverlay(cancellationTokenSource);
                _loadingController.Hide();
                await _loadingController.HideOverlay(cancellationTokenSource);
                
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