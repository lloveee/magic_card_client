using CoreDomain.Scripts.Services.UI.Base;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.Scripts.Mvc.Loading
{
    public class LoadingView : UIView
    {
        private const string k_LoadingBackground = "loading__background";
        private const string k_LoadingProgress = "loading__progress";
        private const string k_LoadingInfo = "loading__info";
        private VisualElement m_LoadingBackground;
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
            m_LoadingProgress = m_TopElement.Q<ProgressBar>(k_LoadingProgress);
            m_LoadingInfo = m_TopElement.Q<Label>(k_LoadingInfo);
        }
    }
}