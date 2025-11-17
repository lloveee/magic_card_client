using System.Threading.Tasks;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.SO;
using CoreDomain.GameDomain.Scripts.State.GamePractice;
using CoreDomain.Scripts.Services.Logger;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Services.StateMachine;
using Cysharp.Threading.Tasks;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home
{
    public class HomeController : IHomeController
    {
        private readonly RankTextureMapSO _rankTextureMap;
        private readonly HomeScreenView _view;
        private readonly IStateMachineService _stateMachine;
        private readonly GamePracticeState.Factory _gamePracticeStateFactory;
        private readonly GameProfileDatabase _database;
        private readonly ISpacetimeServer _spacetimeServer;
        private const string k_HomeView = "HomeScreen";
        private HomeScreenData _data;
        private ILogger _logger;
        public bool IsHidden => _view.IsHidden;
        [Inject]
        public HomeController(RankTextureMapSO rankTextureMap, HomeScreenView view, IStateMachineService stateMachine, ILogger logger
            , GamePracticeState.Factory gamePracticeStateFactory, GameProfileDatabase database, ISpacetimeServer spacetimeServer)
        {
            _rankTextureMap = rankTextureMap;
            _view = view;
            _stateMachine = stateMachine;
            _logger = logger;
            _gamePracticeStateFactory = gamePracticeStateFactory;
            _database = database;
            _spacetimeServer = spacetimeServer;
        }

        public void Initialize()
        {
            _view.Initialize(k_HomeView);
            SetupCallbacks();
        }

        public void InitHomeData(PlayerAccount data)
        {
            _data = new HomeScreenData(data, _rankTextureMap);
            _view.SetDataBinding(_data);
        }

        private void SetupCallbacks()
        {
            _view.SetupCallbacks(OnStartMatchClick, OnCancelMatchClick, OnPracticeClick);
        }

        private void OnPracticeClick(ClickEvent evt)
        {
            TrySwitchToPracticeRoom(_database.Db.CurrentPlayer.Username).Forget();
        }

        private async UniTask TrySwitchToPracticeRoom(string username)
        {
            _logger.Log("Trying to switch to Practice Room");
            var tcs = new UniTaskCompletionSource<bool>();
            void Reducer_On_TryEnqueuePlayer(ReducerEventContext ctx, string u, StatsUnion stats, uint targetPlayerCount, uint totalTime)
            {
                var e = ctx.Event;
                if (e.CallerIdentity == _spacetimeServer.LocalIdentity)
                {
                    if (e.Status is Status.Failed(var error))
                    {
                        _logger.LogError(error);
                        tcs.TrySetResult(false);
                    }
                    else if (e.Status is Status.Committed)
                    {
                        tcs.TrySetResult(true);
                    }
                    _spacetimeServer.Conn.Reducers.OnTryEnqueuePlayer -= Reducer_On_TryEnqueuePlayer;
                }
            } 
            _spacetimeServer.Conn.Reducers.OnTryEnqueuePlayer += Reducer_On_TryEnqueuePlayer;
            
            _spacetimeServer.Conn.Reducers.TryEnqueuePlayer(_database.Db.CurrentPlayer.Username
                , _database.Db.CurrentHero.GetHeroCardData().Stats, 1, 60);

            var res = await tcs.Task;

            if (res)
            {
                _stateMachine.SwitchState(_gamePracticeStateFactory.Create(new GamePracticeInitiatorEnterData(username)));
            }
            else
            {
                _logger.LogError("Failed to switch to Practice Room");
            }
        }

        private void OnCancelMatchClick(ClickEvent evt)
        {
            _data.DecreaseRank(50);
        }

        private void OnStartMatchClick(ClickEvent evt)
        {
            _data.IncreaseRank(50);
        }

        public void ShowView()
        {
            _view.Show();
        }

        public void HideView()
        {
            _view.Hide();
        }
    }
}