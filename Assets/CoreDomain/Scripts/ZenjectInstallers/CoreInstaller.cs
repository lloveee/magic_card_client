using CoreDomain.GameDomain.Scripts.State.GameProfileState;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.Logger;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Services.StateMachine;
using Zenject;

namespace CoreDomain.Scripts.ZenjectInstallers
{
    public class CoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ILogger>().To<UnityLogger>().AsSingle().NonLazy();
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle().NonLazy();
            Container.Bind<IStateMachineService>().To<StateMachineService>().AsSingle().NonLazy();
            Container.Bind<ICommandFactory>().To<CommandFactory>().AsSingle().CopyIntoAllSubContainers().NonLazy();
            Container.BindInterfacesTo<SpacetimeServer>().AsSingle().NonLazy();
            Container.BindInterfacesTo<SceneInitiatorsService>().AsSingle().NonLazy();
        }
    }
}