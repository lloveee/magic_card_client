using System;
using CoreDomain.Scripts.Services.Logger;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.Scripts.Services.UI.Base
{
    public class UIView : IDisposable
    {
        protected bool m_HideOnAwake = true;
        protected bool m_IsOverlay;
        protected VisualElement m_TopElement;
        protected UIDocument m_Document;
        protected ILogger m_logger;
        public VisualElement Root => m_TopElement;
        public bool IsTransparent => m_IsOverlay;
        public bool IsHidden => m_TopElement.style.display == DisplayStyle.None;

        public virtual void Dispose()
        {
        }

        [Inject]
        public UIView(UIDocument document, ILogger logger)
        {
            m_logger = logger;
            m_Document = document;
            //m_logger.Log($"{m_Document == null}");

        }

        /*public virtual void Initialize(string name)
        {

        }*/

        protected virtual void SetVisualElements()
        {

        }

        protected virtual void RegisterButtonCallbacks()
        {

        }

        public virtual void Show()
        {
            m_TopElement.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide()
        {
            m_TopElement.style.display = DisplayStyle.None;
        }

        public virtual void FreezeInterface()
        {

        }

        public virtual void UnFreezeInterface()
        {

        }
    }
}