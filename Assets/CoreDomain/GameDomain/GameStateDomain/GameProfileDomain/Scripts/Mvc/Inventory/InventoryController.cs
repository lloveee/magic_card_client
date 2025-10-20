using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Inventory
{
    public class InventoryController : IInventoryController
    {
        private readonly InventoryScreenView _view;
        private const string k_Inventory_Screen = "InventoryScreen";
        public bool IsHidden => _view.IsHidden;

        public InventoryController(InventoryScreenView view)
        {
            _view = view;
        }
        public void InitInventoryData(List<HeroCardSO> data, GameProfileDatabase.ProfileConfig config)
        {
            _view.InitData(data, config);
        }

        public void Initialize()
        {
            _view.Initialize(k_Inventory_Screen);
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