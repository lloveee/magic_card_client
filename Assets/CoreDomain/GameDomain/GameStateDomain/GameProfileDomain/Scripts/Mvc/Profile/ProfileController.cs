using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Profile
{
    public class ProfileController : IProfileController
    {
        private readonly IHomeController _homeController;
        [Inject]
        public ProfileController(IHomeController homeController)
        {
            _homeController = homeController;
        }

        public void Initialize()
        {
            _homeController.Initialize();
        }
    }
}