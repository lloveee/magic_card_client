using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Inventory;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;
using CoreDomain.Scripts.Services.Logger;
using SpacetimeDB.Types;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Profile
{
    public class ProfileController
    {
        private readonly IHomeController _homeController;
        private readonly IInventoryController _inventoryController;
        private readonly ProfileView _view;
        private readonly ILogger _logger;
        [Inject]
        public ProfileController(IHomeController homeController, IInventoryController inventoryController, ProfileView view, ILogger logger)
        {
            _homeController = homeController;
            _inventoryController = inventoryController;
            _view = view;
            _logger = logger;
        }

        public void Initialize()
        {
            _view.Initialize();
            _homeController.Initialize();
            _inventoryController.Initialize();
        }

        public void InitData(PlayerAccount data)
        {
            _homeController.InitHomeData(data);
        }

        public void InitData(List<HeroCardSO> data, GameProfileDatabase.ProfileConfig config)
        {
            _inventoryController.InitInventoryData(data, config);
        }

        public void RegisterCallbacks()
        {
            _view.RegisterButtons(ActiveHome, ActiveInventory);
        }

        public void UnregisterCallbacks()
        {
            _view.UnregisterButtons(ActiveHome, ActiveInventory);
        }

        private void ActiveHome()
        {
            if (!_homeController.IsHidden) return;
            HideAllViews();
            _homeController.ShowView();
            _logger.Log("home show");
        }

        private void ActiveInventory()
        {
            if (!_inventoryController.IsHidden) return;
            HideAllViews();
            _inventoryController.ShowView();
            _logger.Log("in show");
        }

        private void HideAllViews()
        {
            if (!_inventoryController.IsHidden)
                _inventoryController.HideView();
            if (!_homeController.IsHidden)
                _homeController.HideView();
            _logger.Log("in hide, home hide");
        }
    }
}