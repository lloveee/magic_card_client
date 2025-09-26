using System.Collections.Generic;
using System.Threading;
using CoreDomain.Scripts.CoreInitiator.Base;
using CoreDomain.Scripts.Services.SceneInitiatorService;
using UnityEngine;
using UnityEngine.SceneManagement;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Services.SceneService
{
    public class SceneLoaderService : ISceneLoaderService
    {
        private readonly ILogger _logger;
        private readonly HashSet<string> _loadedScenes = new ();
        private readonly HashSet<string> _loadingScenes = new ();
        private readonly ISceneInitiatorsService _sceneInitiatorsService;

        public SceneLoaderService(ILogger logger, ISceneInitiatorsService sceneInitiatorsService)
        {
            _logger = logger;
            _sceneInitiatorsService = sceneInitiatorsService;
        }
        
        public void InitEntryPoint()
        {
            AddOpenedScenesToLoadedHashset();
        }

        public async Awaitable<bool> TryLoadScene<TEnterData>(SceneType sceneType, TEnterData enterData, CancellationTokenSource cancellationTokenSource) where TEnterData : class, IInitiatorEnterData
        {
            if (!await TryLoadScene(sceneType.ToString(), cancellationTokenSource))
            {
                return false;
            }
            await _sceneInitiatorsService.InvokeInitiatorLoadEntryPoint(sceneType, enterData, cancellationTokenSource);
            return true;
        }
        private async Awaitable<bool> TryLoadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            var isSceneAlreadyLoaded = _loadedScenes.Contains(sceneName);
            if (isSceneAlreadyLoaded)
            {
                _logger.LogError($"scene:[{sceneName}] already loaded");
                return false;
            }

            var isSceneAlreadyLoading = _loadingScenes.Contains(sceneName);
            if (isSceneAlreadyLoading)
            {
                _logger.LogError($"scene:[{sceneName}] is loading");
                return false;
            }
            await LoadScene(sceneName, cancellationTokenSource);
            return true;
        }

        private async Awaitable LoadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            _loadingScenes.Add(sceneName);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            
            // wait scene start() call
            await Awaitable.NextFrameAsync();
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            
            _loadingScenes.Remove(sceneName);
            _loadedScenes.Add(sceneName);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        }

        public async Awaitable StartScene<TEnterData>(SceneType sceneType, TEnterData enterData, CancellationTokenSource cancellationTokenSource) where TEnterData : class, IInitiatorEnterData
        {
            await _sceneInitiatorsService.InvokeInitiatorStartEntryPoint(sceneType, enterData, cancellationTokenSource);
        }

        public async Awaitable<bool> TryUnloadScene(SceneType sceneType, CancellationTokenSource cancellationTokenSource)
        {
            var sceneName = sceneType.ToString();
            var isSceneLoaded = _loadedScenes.Contains(sceneName);
            if (!isSceneLoaded)
            {
                _logger.LogError($"scene:[{sceneName}] is not loaded");
                return false;
            }
            var isSceneLoading = _loadingScenes.Contains(sceneName);
            if (isSceneLoading)
            {
                _logger.LogError($"scene:[{sceneName}] is loading now");
                return false;
            }
            await _sceneInitiatorsService.InvokeInitiatorExitEntryPoint(sceneType, cancellationTokenSource);
            await UnloadScene(sceneName, cancellationTokenSource);
            return true;
        }
        private async Awaitable UnloadScene(string sceneName, CancellationTokenSource cancellationTokenSource)
        {
            await SceneManager.UnloadSceneAsync(sceneName);
            _loadedScenes.Remove(sceneName);
        }
        
        private void AddOpenedScenesToLoadedHashset()
        {
            var countLoaded = SceneManager.sceneCount;

            for (var i = 0; i < countLoaded; i++)
            {
                var sceneName = SceneManager.GetSceneAt(i).name;

                _loadedScenes.Add(sceneName);
            }
        }
    }
}