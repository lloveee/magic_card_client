using CoreDomain.GameDomain.Scripts.Mvc.Login;
using UnityEngine;
using Zenject;

namespace CoreDomain.GameDomain.Scripts.Initiator
{
    public class GameViewInitiator : MonoBehaviour
    {
        private ILoginController _loginController;

        [Inject]
        private void Constructor(ILoginController loginController)
        {
            _loginController = loginController;
        }

        private void Start()
        {
            _loginController.Initialize();
        }
    }
}