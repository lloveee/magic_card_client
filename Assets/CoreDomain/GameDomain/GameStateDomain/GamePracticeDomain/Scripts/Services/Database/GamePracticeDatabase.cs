using System.Collections.Generic;
using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Utils;
using UnityEngine;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Services.Database
{
    public class GamePracticeDatabase
    {
        private readonly ISpacetimeServer _spacetimeServer;
        private readonly ILogger _logger;

        private const string k_player_stats = "player_stats";
        private const string k_match_stats = "match_stats";

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

        public async Awaitable<bool> SubRemoteServer(CancellationTokenSource cancellationTokenSource)
        {
            var cts = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            cts.TrySetResult(true);
            return await cts.Task;
        }

        public async Awaitable<bool> UnsubRemoteServer(CancellationTokenSource cancellationTokenSource)
        {
            var cts = AwaitableUtils.CreateLinkedTcs<bool>(cancellationTokenSource.Token);
            cts.TrySetResult(true);
            return await cts.Task;
        }

        public class DbContext
        {
            public List<HeroCardSO> HeroCardData { get; set; }
        }
    }
}