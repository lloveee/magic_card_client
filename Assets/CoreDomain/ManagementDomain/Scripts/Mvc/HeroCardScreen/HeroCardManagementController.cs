using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.Scripts.Services.Logger;

namespace CoreDomain.ManagementDomain.Scripts.Mvc.HeroCardScreen
{
    public class HeroCardManagementController : IHeroCardManagementController
    {
        private readonly HeroCardManagementView _view;
        private HeroCardDatabase _heroCardDatabase;
        private readonly ILogger _logger;
        private const string k_HeroCardManagementView = "HeroCardManagementScreen";
        public HeroCardManagementController(HeroCardManagementView view, ILogger logger, HeroCardDatabase database)
        {
            _view = view;
            _logger = logger;
            _heroCardDatabase = database;
        }
        public void Initialize()
        {
            _view.Initialize(k_HeroCardManagementView, _heroCardDatabase.data);
            _view.BindingPreviewPanel(_heroCardDatabase.data[0]);
            #if UNITY_EDITOR
            _view.SetupCallbacks(_ => _heroCardDatabase.TryUpdateData(), _ => _heroCardDatabase.TryReUpdateData());
            #endif
        }
        
    }
}