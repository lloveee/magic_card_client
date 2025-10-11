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
        private HeroCardDatabase _heroCardDatabase;
        [Inject]
        private void Constructor(IHeroCardManagementController heroCardManagementController
        , HeroCardDatabase database)
        {
            _heroCardManagementController = heroCardManagementController;
            _heroCardDatabase = database;
        }

        private void Start()
        {
            _heroCardManagementController.Initialize(_heroCardDatabase.data);
        }
    }
}