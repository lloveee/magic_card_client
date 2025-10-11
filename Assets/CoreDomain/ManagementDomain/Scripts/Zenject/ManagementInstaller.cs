using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.ManagementDomain.Scripts.Initiator;
using CoreDomain.ManagementDomain.Scripts.Mvc.HeroCardScreen;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.ManagementDomain.Scripts.Zenject
{
    public class ManagementInstaller : MonoInstaller
    {
        [SerializeField] private HeroCardDatabase heroCardDatabase;
        [SerializeField] private UIDocument managementView;
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ManagementInitiator>().AsSingle().NonLazy();
            Container.Bind<UIDocument>().FromInstance(managementView).AsSingle().NonLazy();
            Container.Bind<HeroCardDatabase>().FromScriptableObject(heroCardDatabase).AsSingle().NonLazy();
            Container.BindInterfacesTo<HeroCardManagementController>().AsSingle().NonLazy();
            Container.Bind<HeroCardManagementView>().AsSingle().NonLazy();
        }
    }
}
