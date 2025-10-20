using System;
using CoreDomain.Scripts.Services.Logger;
using CoreDomain.Scripts.Services.UI.Base;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Profile
{
    public class ProfileView : UIView
    {
        private const string k_HomeButton = "tab_btn__home";
        private const string k_InventoryButton = "tab_btn__inventory";
        private VisualElement m_TabView;
        private Button m_HomeButton;
        private Button m_InventoryButton;
        [Inject]
        public ProfileView(UIDocument document, ILogger logger) : base(document, logger)
        {
        }

        public void Initialize()
        {
            m_TopElement = m_Document.rootVisualElement;
            m_TabView = m_TopElement.Q<VisualElement>("TabScreen");
            SetVisualElements();
        }

        protected override void SetVisualElements()
        {
            m_HomeButton = m_TabView.Q<Button>(k_HomeButton);
            m_InventoryButton = m_TabView.Q<Button>(k_InventoryButton);
        }

        public void RegisterButtons(Action home_callback, Action inventory_callback)
        {
            m_HomeButton.clicked += home_callback;
            m_InventoryButton.clicked += inventory_callback;
        }

        public void UnregisterButtons(Action home_callback, Action inventory_callback)
        {
            m_HomeButton.clicked -= home_callback;
            m_InventoryButton.clicked -= inventory_callback;
        }
    }
}