using System.Threading;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Profile;
using CoreDomain.GameDomain.Scripts.State.GameProfileState;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.CommandFactory;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Utils;
using UnityEngine;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Initiator
{
    public class GameProfileInitiator: IGameProfileInitiator, ISceneInitiator
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ISceneInitiatorsService _sceneInitiatorsService;
        private readonly IHomeController _homeController;

        public GameProfileInitiator(ICommandFactory commandFactory, ISceneInitiatorsService sceneInitiatorsService, IHomeController homeController)
        {
            _commandFactory = commandFactory;
            _homeController = homeController;
            _sceneInitiatorsService = sceneInitiatorsService;
            _sceneInitiatorsService.RegisterInitiator(this);
        }
        public SceneType SceneType => SceneType.GameProfileScene;
        public Awaitable LoadEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            var data = (GameProfileInitiatorEnterData)enterData;
            Debug.Log(data.CurrentPlayer.Nickname);
            _homeController.InitHomeData(data.CurrentPlayer);
            return AwaitableUtils.CompletedTask;
        }

        public Awaitable StartEntryPoint(IInitiatorEnterData enterData, CancellationTokenSource cancellationTokenSource)
        {
            return AwaitableUtils.CompletedTask;
        }

        public Awaitable ExitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            _sceneInitiatorsService.UnregisterInitiator(this);
            return AwaitableUtils.CompletedTask;
        }
    }
}