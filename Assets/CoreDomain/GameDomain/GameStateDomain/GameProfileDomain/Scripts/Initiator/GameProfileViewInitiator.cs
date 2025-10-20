using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Profile;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Initiator
{
    public class GameProfileViewInitiator : MonoBehaviour
    {
        private ProfileController _profileController;
        [Inject]
        private void Constructor(ProfileController profileController)
        {
            _profileController = profileController;
        }

        private void Start()
        {
            _profileController.Initialize();
        }
    }
}