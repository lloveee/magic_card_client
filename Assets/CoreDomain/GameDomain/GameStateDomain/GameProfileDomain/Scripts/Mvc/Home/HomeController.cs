using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.SO;
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
        private const string k_LoginView = "HomeScreen";
        private HomeScreenData _data;
        [Inject]
        public HomeController(RankTextureMapSO rankTextureMap, HomeScreenView view)
        {
            _rankTextureMap = rankTextureMap;
            _view = view;
        }

        public void Initialize()
        {
            _view.Initialize(k_LoginView);
            SetupCallbacks();
        }

        public void InitHomeData(PlayerAccount data)
        {
            _data = new HomeScreenData(data, _rankTextureMap);
            _view.SetDataBinding(_data);
        }

        private void SetupCallbacks()
        {
            _view.SetupCallbacks(OnStartMatchClick, OnCancelMatchClick);
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
            _view.Hide();
        }

        public void HideView()
        {
            _view.Show();
        }
    }
}