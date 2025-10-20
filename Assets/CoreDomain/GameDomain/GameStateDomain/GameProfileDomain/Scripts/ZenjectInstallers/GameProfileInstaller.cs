using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Initiator;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Initiator;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Inventory;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Profile;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.SO;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.ZenjectInstallers
{
    public class GameProfileInstaller : MonoInstaller
    {
        [SerializeField] private RankTextureMapSO rankTextureMap;
        [SerializeField] private UIDocument gameView;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameProfileInitiator>().AsSingle().NonLazy();
            Container.Bind<ProfileController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<HomeController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<InventoryController>().AsSingle().NonLazy();
            Container.Bind<GameProfileDatabase>().AsSingle().NonLazy();
            Container.Bind<UIDocument>().FromInstance(gameView).AsSingle().NonLazy();
            Container.Bind<HomeScreenView>().AsSingle().NonLazy();
            Container.Bind<InventoryScreenView>().AsSingle().NonLazy();
            Container.Bind<ProfileView>().AsSingle().NonLazy();
            Container.Bind<RankTextureMapSO>().FromScriptableObject(rankTextureMap).AsSingle().NonLazy();
        }
    }
}