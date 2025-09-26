using System.Threading;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.SceneService;
using UnityEngine;

namespace CoreDomain.Scripts.Services.SceneInitiatorService
{
    /// <summary>
    /// managing scene loading with fsm
    /// </summary>
    public interface ISceneInitiatorsService
    {
        void RegisterInitiator(ISceneInitiator sceneInitiator);
        void UnregisterInitiator(ISceneInitiator sceneInitiator);
        Awaitable InvokeInitiatorLoadEntryPoint(SceneType sceneType, IInitiatorEnterData enterData, 
            CancellationTokenSource cancellationTokenSource);
        Awaitable InvokeInitiatorStartEntryPoint(SceneType sceneType, IInitiatorEnterData enterData, 
            CancellationTokenSource cancellationTokenSource);
        Awaitable InvokeInitiatorExitEntryPoint(SceneType sceneType, CancellationTokenSource cancellationTokenSource);
    }
}