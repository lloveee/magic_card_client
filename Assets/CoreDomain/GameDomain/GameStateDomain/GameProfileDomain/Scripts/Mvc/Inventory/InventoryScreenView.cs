using System;
using System.Collections.Generic;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Services.Database;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Mvc.Controls;
using CoreDomain.Scripts.Services.Logger;
using CoreDomain.Scripts.Services.UI.Base;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Inventory
{
    public class InventoryScreenView : UIView
    {
        private const string k_InventoryScrollView = "inventory__view_container";
        private const string k_InventoryPanelScrollView = "inventory__panel__info_container";
        private const string k_InventoryPanelInfo = "inventory_panel__info_item";
        private const string k_InventoryPanelTitle = "inventory__panel_header_text";
        private const string k_InventorySetActiveButton = "inventory__panel_set_btn";
        
        private ScrollView m_InventoryScrollView;
        private ScrollView m_InventoryPanelScrollView;
        private Label m_InventoryPanelTitle;
        private Button m_InventorySetActiveButton;
        private GameProfileDatabase.ProfileConfig m_config;
        private int m_CurrentData;
        private HeroCardPreviewElement _lastPreviewElement = null;
        
        [Inject]
        public InventoryScreenView(UIDocument document, ILogger logger) : base(document, logger)
        {
        }

        public void Initialize(string name)
        {
            m_TopElement = m_Document.rootVisualElement.Q<VisualElement>(name);
            SetVisualElements();
            Hide();
        }

        protected override void SetVisualElements()
        {
            m_InventoryScrollView = m_TopElement.Q<ScrollView>(k_InventoryScrollView);
            m_InventoryPanelScrollView = m_TopElement.Q<ScrollView>(k_InventoryPanelScrollView);
            m_InventoryPanelTitle = m_TopElement.Q<Label>(k_InventoryPanelTitle);
            m_InventorySetActiveButton = m_TopElement.Q<Button>(k_InventorySetActiveButton);
        }

        public void InitData(List<HeroCardSO> data, GameProfileDatabase.ProfileConfig config)
        {
            m_config = config;
            m_CurrentData = config.CurrentHeroIndex;
            m_InventorySetActiveButton.SetEnabled(false);
            for (int i = 0; i < data.Count; i++)
            {
                var cardVisual = new HeroCardPreviewElement
                {
                    HeroCardSO = data[i],
                    Index = i
                };
                cardVisual.OnClickedHeroCard += OnPreviewElementClickedHeroCard;
                m_InventoryScrollView.contentContainer.Add(cardVisual);
                if (m_CurrentData == i)
                {
                    _lastPreviewElement = cardVisual;
                    cardVisual.SetSelected(true);
                }
            }
            m_InventorySetActiveButton.clicked += OnSetActive;
            m_InventoryPanelTitle.text = data[m_CurrentData].cardName;
            m_InventoryPanelScrollView.contentContainer.Clear();
            foreach (var skill in data[m_CurrentData].GetSkillDescription())
            {
                var label = m_InventoryPanelScrollView.contentContainer.CreateChildWithName<Label>(k_InventoryPanelInfo);
                label.AddToClassList(k_InventoryPanelInfo);
                label.text = skill;
            }
        }

        private void OnPreviewElementClickedHeroCard(HeroCardPreviewElement target)
        {
            if (target == _lastPreviewElement) return;
            var data = target.HeroCardSO;
            var index = target.Index;
            m_InventorySetActiveButton.SetEnabled(m_config.CurrentHeroIndex != index);
            if (_lastPreviewElement != null) _lastPreviewElement.SetSelected(false);
            _lastPreviewElement = target;
            target.SetSelected(true);
            m_InventoryPanelTitle.text = data.cardName;
            m_InventoryPanelScrollView.contentContainer.Clear();
            foreach (var skill in data.GetSkillDescription())
            {
                var label = m_InventoryPanelScrollView.contentContainer.CreateChildWithName<Label>(k_InventoryPanelInfo);
                label.AddToClassList(k_InventoryPanelInfo);
                label.text = skill;
            }
            m_CurrentData = index;
        }

        private void OnSetActive()
        {
            m_config.CurrentHeroIndex = m_CurrentData;
            m_InventorySetActiveButton.SetEnabled(false);
        }
    }
}