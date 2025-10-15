using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Utils;
using SpacetimeDB;
using SpacetimeDB.BSATN;
using SpacetimeDB.Types;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database
{
    public class GameProfileDatabase
    {
        private readonly ISpacetimeServer _spacetimeServer;
        private readonly ILogger _logger;
        private const string k_player_account = "player_account";
        public DbContext Db { get; private set; }

        public GameProfileDatabase(ISpacetimeServer spacetimeServer, ILogger logger)
        {
            _spacetimeServer = spacetimeServer;
            _logger = logger;
            Db = new DbContext();
        }

        public async Awaitable<bool> SubRemoteServer(string username, CancellationTokenSource cancellationTokenSource)
        {
            var cts = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            string player_account_sub_query = $"SELECT * FROM {k_player_account} c WHERE c.Username = '{username}'";
            _spacetimeServer.SubscribeTableWithId(k_player_account, new string[]{player_account_sub_query}, 
                context => OnPlayerAccountSubApply(context, cts),
                (context, exception) => OnPlayerAccountSubError(context, exception, cts));
            return await cts.Task;
        }
        
        public async Awaitable<bool> UnsubRemoteServer(CancellationTokenSource cancellationTokenSource)
        {
            var cts = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            _spacetimeServer.UnsubscribeTableWithId(k_player_account,context => OnUnapplyCallback(context, cts));
            return await cts.Task;
        }

        private void OnUnapplyCallback(SubscriptionEventContext obj, TaskCompletionSource<bool> tcs)
        {
            _spacetimeServer.Conn.Db.PlayerAccount.OnUpdate -= PlayerAccountOnOnUpdate;
            tcs.TrySetResult(true);
        }

        private void OnPlayerAccountSubError(ErrorContext ctx, Exception e, TaskCompletionSource<bool> tcs)
        {
            _logger.LogWarning("Network error");
            tcs.TrySetException(e);
        }

        private void OnPlayerAccountSubApply(SubscriptionEventContext ctx, TaskCompletionSource<bool> tcs)
        {
            var p = ctx.Db.PlayerAccount.Iter().FirstOrDefault();
            Db.CurrentPlayer = p;
            if (p == null)
            {
                tcs.TrySetResult(false);
                return;
            }
            _spacetimeServer.Conn.Db.PlayerAccount.OnUpdate += PlayerAccountOnOnUpdate;
            tcs.TrySetResult(true);
        }

        private void PlayerAccountOnOnUpdate(EventContext context, PlayerAccount _, PlayerAccount newRow)
        {
            Db.CurrentPlayer = newRow;
        }

        public class DbContext
        {
            public PlayerAccount CurrentPlayer { get; set; }
        }
    }
}