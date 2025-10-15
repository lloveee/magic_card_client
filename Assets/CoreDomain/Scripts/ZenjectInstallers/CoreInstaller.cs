using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.Scripts.Mvc.Loading;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.DataPersistence;
using CoreDomain.Scripts.Services.Logger;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.Serializer;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Services.StateMachine;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.ZenjectInstallers
{
    public class CoreInstaller : MonoInstaller
    {
        [SerializeField] private HeroCardDatabase heroCardDatabase;
        [SerializeField] private UIDocument coreViewDocument;
        public override void InstallBindings()
        {
            Container.Bind<ILogger>().To<UnityLogger>().AsSingle().NonLazy();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle().NonLazy();
            Container.Bind<IStateMachineService>().To<StateMachineService>().AsSingle().NonLazy();
            Container.Bind<ICommandFactory>().To<CommandFactory>().AsSingle().CopyIntoAllSubContainers().NonLazy();
            Container.Bind<HeroCardDatabase>().FromScriptableObject(heroCardDatabase).AsSingle().NonLazy();
            Container.BindInterfacesTo<LoadingController>().AsSingle().NonLazy();
            Container.Bind<LoadingView>().AsSingle().NonLazy();
            Container.Bind<UIDocument>().FromInstance(coreViewDocument).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StdbStdbSerializerService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StdbHeroCardDataPersistence>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SpacetimeServer>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SceneInitiatorsService>().AsSingle().NonLazy();
        }
    }
}