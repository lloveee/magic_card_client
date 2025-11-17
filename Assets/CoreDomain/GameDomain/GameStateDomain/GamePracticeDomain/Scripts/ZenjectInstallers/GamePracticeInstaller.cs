using CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Initiator;
using CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.Services.Database;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePracticeDomain.Scripts.ZenjectInstallers
{
    public class GamePracticeInstaller : MonoInstaller
    {
        [SerializeField] private UIDocument gameView;
        public override void InstallBindings()
        {
            Container.Bind<UIDocument>().FromInstance(gameView).AsSingle().NonLazy();
            Container.BindInterfacesTo<GamePracticeInitiator>().AsSingle().NonLazy();
            Container.Bind<GamePracticeDatabase>().AsSingle().NonLazy();
        }
    }
}