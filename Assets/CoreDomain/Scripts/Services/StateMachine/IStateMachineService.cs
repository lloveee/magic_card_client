using System.Threading;
using UnityEngine;

namespace CoreDomain.Scripts.Services.StateMachine
{
    public interface IStateMachineService
    {
        IGameState CurrentState { get;}
        Awaitable EnterInitialState(IGameState initialState, CancellationTokenSource cancellationTokenSource);
        void SwitchState(IGameState nextState);
    }
}