using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Utils;
using SpacetimeDB.Types;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Services.Database
{
    public class GamePracticeDatabase
    {
        private readonly ISpacetimeServer _spacetimeServer;
        private readonly ILogger _logger;

        private const string k_match_key = "match";
        private const string k_player_context = "player_context";
        private const string k_match_context = "match_context";

        public DbContext Db { get; private set; }
        

        public GamePracticeDatabase(ISpacetimeServer spacetimeServer, ILogger logger, HeroCardDatabase heroCardDatabase)
        {
            _spacetimeServer = spacetimeServer;
            _logger = logger;
            Db = new DbContext
            {
                HeroCardData = heroCardDatabase.data
            };
        }

        public async Awaitable<bool> SubRemoteServer(string username, CancellationTokenSource cancellationTokenSource)
        {
            var cts = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            string match_sub_player_query = $@"
                    SELECT 
                        p.*
                    FROM {k_player_context} p
                    JOIN {k_match_context} m ON p.MatchId = m.MatchId
                    WHERE p.Username = '{username}'";
            
            string match_sub_match_query = $@"
                    SELECT 
                        m.*
                    FROM {k_player_context} p
                    JOIN {k_match_context} m ON p.MatchId = m.MatchId
                    WHERE p.Username = '{username}'";
            
            _spacetimeServer.SubscribeTableWithId(k_match_key, new []{match_sub_player_query, match_sub_match_query},
                context => OnMatchSub(context, cts),
                (context, exception) => OnMatchSubError(context, exception, cts));
            return await cts.Task;
        }

        public async Awaitable<bool> UnsubRemoteServer(CancellationTokenSource cancellationTokenSource)
        {
            var cts = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            _spacetimeServer.UnsubscribeTableWithId(k_match_key, context => OnUnapplyCallback(context, cts));
            return await cts.Task;
        }
        
        private void OnUnapplyCallback(SubscriptionEventContext obj, TaskCompletionSource<bool> tcs)
        {
            //_spacetimeServer.Conn.Db.PlayerAccount.OnUpdate -= PlayerAccountOnOnUpdate;
            tcs.TrySetResult(true);
        }
        
        private void OnMatchSubError(ErrorContext ctx, Exception e, TaskCompletionSource<bool> tcs)
        {
            _logger.LogWarning("Network error");
            tcs.TrySetException(e);
        }

        private void OnMatchSub(SubscriptionEventContext ctx, TaskCompletionSource<bool> tcs)
        {
            var p = ctx.Db.PlayerContext.Iter().FirstOrDefault();
            var m = ctx.Db.MatchContext.Iter().FirstOrDefault();
            Db.SelfContext = p;
            Db.MatchContext = m;
            if (p == null || m == null)
            {
                tcs.TrySetResult(false);
                return;
            }

            _spacetimeServer.Conn.Db.PlayerContext.OnUpdate += PlayerContextUpdate;
            _spacetimeServer.Conn.Db.MatchContext.OnUpdate += MatchContextUpdate;
            tcs.TrySetResult(true);
        }

        private void MatchContextUpdate(EventContext context, MatchContext oldRow, MatchContext newRow)
        {
            Db.MatchContext = newRow;
        }

        private void PlayerContextUpdate(EventContext ctx, PlayerContext oldRow, PlayerContext newRow)
        {
            Db.SelfContext = newRow;
        }
        
        public class DbContext
        {
            public List<HeroCardSO> HeroCardData { get; set; }
            public MatchContext MatchContext { get; set; }
            public PlayerContext SelfContext { get; set; }
            public MatchConfig Config { get; set; }
        }

        public class MatchConfig
        {
            public CardTypeWeight CardTypeWeight { get; set; }
            public CardFactionWeight CardFactionWeight { get; set; }
            public CardLevelWeight CardLevelWeight { get; set; }
        }
    }
}