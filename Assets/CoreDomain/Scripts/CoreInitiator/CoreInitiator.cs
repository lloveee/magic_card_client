using System;
using System.Threading;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Mvc.Loading;
using CoreDomain.Scripts.Services.SceneService;
using CoreDomain.Scripts.Services.SpacetimeServer;
using CoreDomain.Scripts.Utils;
using SpacetimeDB;
using SpacetimeDB.Types;
using UnityEngine;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.CoreInitiator
{
    public class CoreInitiator : MonoBehaviour
    {
        private ISceneLoaderService _sceneLoaderService;
        private ISpacetimeServer _spacetimeServer;
        private ILogger _logger;
        private ILoadingController _loadingController;
        [SerializeField] private ServerConfig config;
        [SerializeField] private bool useProxy = false;
        [SerializeField] private SceneType proxyScene = SceneType.GameProfileScene;
        [SerializeReference, SubclassSelector] private IInitiatorEnterData mockData;

        [Inject]
        private void Constructor(ISceneLoaderService sceneLoaderService, ILogger logger, ISpacetimeServer spacetimeServer, ILoadingController loadingController)
        {
            _spacetimeServer = spacetimeServer;
            _sceneLoaderService = sceneLoaderService;
            _loadingController = loadingController;
            _logger = logger;
        }

        private void Start()
        {
            _logger.Log(System.Net.Dns.GetHostAddresses("20250430.xyz")[0].ToString());
            //_ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
            _loadingController.Initialize();
            if (!useProxy)
            {
                _loadingController.Show();
                _loadingController.SetInfo("Connecting Server ...");
                _loadingController.SetProgress(0.5f);
                _spacetimeServer.InitializeConnection(config.url, config.module, OnConnected, OnConnectError, OnDisconnected);
            }
            
            else
            {
                _ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken));
            }
        }

        private void OnDisconnected(DbConnection conn, Exception e)
        {
            _logger.Log("Disconnected");
        }

        private void OnConnectError(Exception e)
        {
            _loadingController.SetInfo("Connecting Server Error!");
            _logger.LogError(e.Message);
        }

        private void OnConnected(DbConnection conn, Identity identity, string token)
        {
            _spacetimeServer.LocalIdentity = identity;
            AuthToken.SaveToken(token);
            _logger.Log("Connected STDB");
            _ = InitEntryPoint(CancellationTokenSource.CreateLinkedTokenSource(Application.exitCancellationToken), identity);
        }


        private async Awaitable InitEntryPoint(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                UpdateApplicationSettings();
                InitializeServices();
                await LoadGameScene(cancellationTokenSource);
                await _loadingController.ShowOverlay(cancellationTokenSource);
                _loadingController.Hide();
                await _loadingController.HideOverlay(cancellationTokenSource);
            }
            catch (OperationCanceledException)
            {
                _logger.Log("Operation init core was cancelled");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
        private async Awaitable InitEntryPoint(CancellationTokenSource cancellationTokenSource, Identity identity)
        {
            try
            {
                UpdateApplicationSettings();
                InitializeServices();
                await LoadGameScene(cancellationTokenSource, identity);
            }
            catch (OperationCanceledException)
            {
                _logger.Log("Operation init core was cancelled");
            }
            catch (Exception e)
            {
                _loadingController.SetInfo(e.Message);
                _logger.LogError(e.Message);
                throw;
            }
        }
        
        private void UpdateApplicationSettings()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }
        
        private void InitializeServices()
        {
            _sceneLoaderService.InitEntryPoint();
        }
        
        private async Awaitable LoadGameScene(CancellationTokenSource cancellationTokenSource, Identity identity)
        {
            await _sceneLoaderService.TryLoadScene(SceneType.GameScene, new GameInitiatorEnterData(identity), cancellationTokenSource);
        }
        
        private async Awaitable LoadGameScene(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                await _sceneLoaderService.TryLoadScene(proxyScene, mockData, cancellationTokenSource);
            }
            catch (Exception ex)
            {
                _logger.LogError($"TryLoadScene failed: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }
    }
}