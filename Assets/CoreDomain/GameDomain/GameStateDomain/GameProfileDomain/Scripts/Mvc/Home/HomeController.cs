using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.SO;
using CoreDomain.Scripts.Services.StateMachine;
using SpacetimeDB.Types;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home
{
    public class HomeController : IHomeController
    {
        private readonly RankTextureMapSO _rankTextureMap;
        private readonly HomeScreenView _view;
        private readonly IStateMachineService _stateMachine;
        private const string k_HomeView = "HomeScreen";
        private HomeScreenData _data;
        public bool IsHidden => _view.IsHidden;
        [Inject]
        public HomeController(RankTextureMapSO rankTextureMap, HomeScreenView view, IStateMachineService stateMachine)
        {
            _rankTextureMap = rankTextureMap;
            _view = view;
            _stateMachine = stateMachine;
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
            //TODO:Switch To Practice Room
            //_stateMachine.SwitchState();
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