using System.Threading;
using CoreDomain.Scripts.Services.SceneService;
using UnityEngine;

namespace CoreDomain.Scripts.CoreInitiator.Base
{
    public interface ISceneInitiator
    {
        SceneType SceneType { get; }
        Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource);
        Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource);
        Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource);
    }
}