using System.Threading;
using UnityEngine;

namespace CoreDomain.Scripts.Services.CommandFactory
{
    public interface ICommandAsyncWithResult<TResult> : IBaseCommand
    {
        Awaitable<TResult> Execute(CancellationTokenSource cancellationTokenSource);
    }
}