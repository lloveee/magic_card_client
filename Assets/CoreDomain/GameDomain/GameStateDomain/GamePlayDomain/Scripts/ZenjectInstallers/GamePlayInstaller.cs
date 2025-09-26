using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.Initiator;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.ZenjectInstallers
{
    public class GamePlayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GamePlayInitiator>().AsSingle().NonLazy();
        }
    }
}