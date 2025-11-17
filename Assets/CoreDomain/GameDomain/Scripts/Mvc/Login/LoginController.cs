using System;
using System.Threading;
using CoreDomain.GameDomain.Scripts.State.GameProfile;
using CoreDomain.Scripts.Mvc.Loading;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.DataPersistence;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Services.StateMachine;
using Cysharp.Threading.Tasks;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.Scripts.Mvc.Login
{
    public class LoginController : ILoginController
    {
        private readonly LoginView m_view;
        private readonly PlayerInitView m_playerInit_View;
        private readonly ISpacetimeServer _spacetimeServer;
        private readonly ILogger _logger;
        private readonly ICommandFactory _commandFactory;
        private readonly ILoadingController _loadingController;
        private readonly GameProfileState.Factory _gameProfileStateFactory;
        private readonly IStateMachineService _stateMachine;
        private readonly PlayerPrefsDataPersistence _dataPersistence;
        private const string k_player_account = "player_account";
        private const string k_LoginView = "LoginScreen";
        private const string k_InitView = "InitScreen";
        private const string k_login_config = "player_login";
        private LoginConfig _loginConfig;
        private class LoginConfig
        {
            public LoginConfig(string username, string password, bool auto)
            {
                this.username = username;
                this.password = password;
                this.auto = auto;
            }
            public string username;
            public string password;
            public bool auto;
        }
        
        [Inject]
        public LoginController(LoginView view,PlayerInitView initView, ISpacetimeServer spacetimeServer, ILogger logger, ICommandFactory commandFactory, 
            IStateMachineService stateMachine, GameProfileState.Factory gameProfileStateFactory, ILoadingController loadingController, 
            PlayerPrefsDataPersistence dataPersistence)
        {
            _loadingController = loadingController;
            m_view = view;
            m_playerInit_View = initView;
            _logger = logger;
            _stateMachine = stateMachine;
            _gameProfileStateFactory = gameProfileStateFactory;
            _spacetimeServer = spacetimeServer;
            _commandFactory = commandFactory;
            _dataPersistence = dataPersistence;
        }

        public void Initialize()
        {
            _loginConfig = _dataPersistence.Load<LoginConfig>(k_login_config);
            
            m_view.Initialize(k_LoginView);
            m_playerInit_View.Initialize(k_InitView);
            m_view.Hide();
            m_playerInit_View.Hide();
            SetupCallbacks();
        }

        public void TryAutoLogin()
        {
            if (_loginConfig != null)
            {
                m_view.SetAutoLogin(_loginConfig.auto);
                m_view.SetLoginContext(_loginConfig.username, _loginConfig.password);
                if (_loginConfig.auto)
                {
                    m_view.FreezeInterface();
                    _spacetimeServer.Conn.Reducers.AuthLogin(_loginConfig.username, _loginConfig.password);
                }
            }
        }

        private void SetupCallbacks()
        {
            m_view.FreezeInterface();
            m_view.SetupCallbacks(OnLoginButtonClicked);
            m_playerInit_View.SetupCallbacks(OnInitButtonClicked);
            _spacetimeServer.Conn.Reducers.OnAuthLogin += Reducer_OnAuthLogin;
            m_view.UnFreezeInterface();
        }

        private void OnInitButtonClicked(ClickEvent evt)
        {
            m_playerInit_View.FreezeInterface();
            InitializePlayer();
        }

        private void OnLoginButtonClicked(ClickEvent evt)
        {
            m_view.FreezeInterface();
            var c = m_view.GetLoginContext();
            var b = m_view.GetAutoLogin();
            _loginConfig = new LoginConfig(c.username, c.password, b);
            _dataPersistence.Save(k_login_config, _loginConfig);
            _spacetimeServer.Conn.Reducers.AuthLogin(c.username, c.password);
        }

        private void Reducer_OnAuthLogin(ReducerEventContext ctx, string username, string password)
        {
            var e = ctx.Event;
            if (e.CallerIdentity == _spacetimeServer.LocalIdentity)
            {
                if (e.Status is Status.Failed(var error))
                {
                    _logger.LogError(error);
                    m_view.UnFreezeInterface();
                }
                else if (e.Status is Status.Committed)
                {
                    SubscribePlayerAccount(username);
                }
            }
        }

        private void SubscribePlayerAccount(string username)
        {
            string player_account_sub_query = $"SELECT * FROM {k_player_account} p WHERE p.Username = '{username}'";
            _spacetimeServer.SubscribeTableWithId(k_player_account, new []{player_account_sub_query}
                ,c => OnPlayerAccountSubApply(c, username)
                , OnPlayerAccountSubError);
        }

        private void OnPlayerAccountSubError(ErrorContext ctx, Exception e)
        {
            _logger.LogError(e.Message);
            UnfreezeInterface();
        }

        private void InitializePlayer()
        {
            var lc = m_view.GetLoginContext();
            var rc = m_playerInit_View.GetPlayerInitContext();
            _spacetimeServer.Conn.Reducers.Initialize(lc.username, rc.nickname);
        }
        
        private void Reducer_InitializePlayer(ReducerEventContext ctx, string username, string _)
        {
            var e = ctx.Event;
            if (e.CallerIdentity == _spacetimeServer.LocalIdentity)
            {
                if (e.Status is Status.Failed(var error))
                {
                    _logger.LogError(error);
                    m_playerInit_View.UnFreezeInterface();
                }
                else if (e.Status is Status.Committed)
                {
                    SubscribePlayerAccount(username);
                    _spacetimeServer.Conn.Reducers.OnInitialize -= Reducer_InitializePlayer;
                }
            }
        }

        private void OnPlayerAccountSubApply(SubscriptionEventContext ctx, string username)
        {
            var player = ctx.Db.PlayerAccount.Username.Find(username);
            if (player == null)
            {
                UnfreezeInterface();
                //_logger.LogWarning("Something went wrong. Please try again");
                _spacetimeServer.UnsubscribeTableWithId(k_player_account, context =>
                {
                    _logger.Log("Try Show INIT View");
                    m_view.Hide();
                    //first create account
                    m_playerInit_View.Show();
                    _spacetimeServer.Conn.Reducers.OnInitialize += Reducer_InitializePlayer;
                });
                return;
            } 
            LoadingHome(player).Forget();
        }

        private async UniTask LoadingHome(PlayerAccount player)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken);
            await _loadingController.ShowOverlay(cts);
            HideView();
            m_playerInit_View.Hide();
            _logger.Log($"Logged in ");
            _loadingController.Show();
            await _loadingController.HideOverlay(cts);
            _spacetimeServer.UnsubscribeTableWithId(k_player_account, context =>
            {
                _logger.Log("UnsubTable View");
            });
            await _stateMachine.EnterInitialState(
                _gameProfileStateFactory.Create(new GameProfileInitiatorEnterData(player)), cts);
            
            await _loadingController.ShowOverlay(cts);
            _loadingController.Hide();
            await _loadingController.HideOverlay(cts);
        }

        public void FreezeInterface()
        {
            if (m_view.IsHidden) m_playerInit_View.FreezeInterface();
            else m_view.FreezeInterface();
        }

        public void UnfreezeInterface()
        {
            if (m_view.IsHidden) m_playerInit_View.UnFreezeInterface();
            else m_view.UnFreezeInterface();
        }

        public void HideView()
        {
            m_view.Hide();
        }

        public void ShowView()
        {
            m_view.Show();
        }
    }
}