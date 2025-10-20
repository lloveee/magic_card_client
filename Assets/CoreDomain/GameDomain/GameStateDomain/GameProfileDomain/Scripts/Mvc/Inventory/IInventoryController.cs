using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Inventory
{
    public interface IInventoryController
    {
        public void InitInventoryData(List<HeroCardSO> data, GameProfileDatabase.ProfileConfig config);
        void Initialize();
        void ShowView();
        void HideView();
        bool IsHidden { get; }
    }
}