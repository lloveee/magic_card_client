using System.Threading;
using CoreDomain.Scripts.Services.UI.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Mvc.Loading
{
    public class LoadingView : UIView
    {
        private const string k_LoadingContainer = "loading__container";
        private const string k_LoadingBackground = "loading__background";
        private const string k_LoadingProgress = "loading__progress";
        private const string k_LoadingInfo = "loading__info";
        private const string k_LoadingOverlay = "overlay";
        private const string k_OverlayType = "overlay__basic_fade";
        private const string k_LoadingActiveOverlay = "overlay__active";
        private const int k_overlay_ms = 500;
        private VisualElement m_LoadingContainer;
        private VisualElement m_LoadingBackground;
        private VisualElement m_LoadingOverlay;
        private ProgressBar m_LoadingProgress;
        private Label m_LoadingInfo;
        
        [Inject]
        public LoadingView(UIDocument document, ILogger logger) : base(document, logger)
        {
        }

        public void Initialize()
        {
            m_TopElement = m_Document.rootVisualElement;
            SetVisualElements();
            RegisterButtonCallbacks();
            m_LoadingOverlay.AddToClassList(k_OverlayType);
        }

        public override void Hide()
        {
            m_LoadingContainer.style.display = DisplayStyle.None;
        }

        public override void Show()
        {
            m_LoadingContainer.style.display = DisplayStyle.Flex;
        }

        public async UniTask DisplayOverlay(CancellationTokenSource cts)
        {
            m_LoadingOverlay.AddToClassList(k_LoadingActiveOverlay);
            await UniTask.Delay(k_overlay_ms, cancellationToken:cts.Token);
        } 
        
        public async UniTask HideOverlay(CancellationTokenSource cts)
        {
            m_LoadingOverlay.RemoveFromClassList(k_LoadingActiveOverlay);
            await UniTask.Delay(k_overlay_ms, cancellationToken:cts.Token);
        } 

        public void SetProgressValue(float value)
        {
            value = Mathf.Clamp01(value);
            m_LoadingProgress.value = value;
        }

        public void SetInfo(string info)
        {
            m_LoadingInfo.text = info;
        }

        public void SetBackground(Texture2D texture)
        {
            m_LoadingBackground.style.backgroundImage = texture;
        }

        protected override void SetVisualElements()
        {
            m_LoadingBackground = m_TopElement.Q<VisualElement>(k_LoadingBackground);
            m_LoadingContainer = m_TopElement.Q<VisualElement>(k_LoadingContainer);
            m_LoadingProgress = m_TopElement.Q<ProgressBar>(k_LoadingProgress);
            m_LoadingInfo = m_TopElement.Q<Label>(k_LoadingInfo);
            m_LoadingOverlay = m_TopElement.Q<VisualElement>(k_LoadingOverlay);
        }
    }
}