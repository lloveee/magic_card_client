using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Services.Logger;

namespace CoreDomain.ManagementDomain.Scripts.Mvc.HeroCardScreen
{
    public class HeroCardManagementController : IHeroCardManagementController
    {
        private readonly HeroCardManagementView _view;
        private readonly ILogger _logger;
        private const string k_HeroCardManagementView = "HeroCardManagementScreen";
        public HeroCardManagementController(HeroCardManagementView view, ILogger logger)
        {
            _view = view;
            _logger = logger;
        }
        public void Initialize(List<HeroCardSO> cards)
        {
            _view.Initialize(k_HeroCardManagementView, cards);
            _view.BindingPreviewPanel(cards[0]);
        }
    }
}