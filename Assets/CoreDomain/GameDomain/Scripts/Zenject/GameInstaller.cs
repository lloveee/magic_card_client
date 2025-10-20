using CoreDomain.GameDomain.Scripts.Initiator;
using CoreDomain.GameDomain.Scripts.Mvc.Login;
using CoreDomain.GameDomain.Scripts.State.GamePlay;
using CoreDomain.GameDomain.Scripts.State.GameProfile;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.Scripts.Zenject
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIDocument gameView;
        public override void InstallBindings()
        {
            Container.BindFactory<GamePlayInitiatorEnterData, GamePlayState, GamePlayState.Factory>()
                .AsSingle().NonLazy();
            Container.BindFactory<GameProfileInitiatorEnterData, GameProfileState, GameProfileState.Factory>()
                .AsSingle().NonLazy();
            Container.BindInterfacesTo<GameInitiator>().AsSingle().NonLazy();

            Container.Bind<UIDocument>().FromInstance(gameView).AsSingle().NonLazy();

            Container.Bind<LoginView>().AsSingle().NonLazy();
            Container.Bind<PlayerInitView>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LoginController>().AsSingle().NonLazy();
        }
    }
}