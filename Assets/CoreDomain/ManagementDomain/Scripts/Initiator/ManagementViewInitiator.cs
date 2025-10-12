using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData;
using CoreDomain.ManagementDomain.Scripts.Mvc.HeroCardScreen;
using UnityEngine;
using Zenject;

namespace CoreDomain.ManagementDomain.Scripts.Initiator
{
    public class ManagementViewInitiator : MonoBehaviour
    {
        private IHeroCardManagementController _heroCardManagementController;
        
        [Inject]
        private void Constructor(IHeroCardManagementController heroCardManagementController)
        {
            _heroCardManagementController = heroCardManagementController;
        }

        private void Start()
        {
            _heroCardManagementController.Initialize();
        }
    }
}