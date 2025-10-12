using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.Scripts.Extensions;

namespace CoreDomain.Scripts.Utils
{
    public static class AwaitableUtils
    {
        static readonly AwaitableCompletionSource completionSource = new AwaitableCompletionSource();

        public static Awaitable CompletedTask
        {
            get
            {
                completionSource.SetResult();
                var awaitable = completionSource.Awaitable;
                completionSource.Reset();
                return awaitable;
            }
        }

        public static async Awaitable WaitUntil(Func<bool> condition, CancellationToken cancellationToken)
        {
            while (!condition())
            {
                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
        
        public static async Awaitable WhenAny(this Awaitable[] tasks, CancellationToken cancellationToken)
        {
            if (tasks.IsNullOrEmpty()) return;
            var length = tasks.Length;
            while (true)
            {
                for (int i = 0; i < length; i++)
                {
                    if (tasks[i].GetAwaiter().IsCompleted)
                        return;
                }

                await Awaitable.NextFrameAsync(cancellationToken);
            }
        }
        
        public static TaskCompletionSource<T> CreateLinkedTcs<T>(CancellationToken token)
        {
            var tcs = new TaskCompletionSource<T>();
            token.Register(() => tcs.TrySetCanceled());
            return tcs;
        }
    }
}