using System;
using System.Collections.Generic;
using System.Reflection;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Extensions;
using CoreDomain.Scripts.Mvc.Controls;
using CoreDomain.Scripts.Services.UI.Base;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.ManagementDomain.Scripts.Mvc.HeroCardScreen
{
    public class HeroCardManagementView : UIView
    {
        private const string k_HeroCardPreviewList = "hero_card_pre__list";
        private const string k_HeroCardPreviewPanel = "hero_card_pre__panel";
        private const string k_HeroCardPreviewAttr = "hero_card_pre__edit_attr";
        private const string k_HeroCardPreviewAttrList = "hero_card_pre__edit_list";

        private VisualElement m_HeroCardContainer;
        private VisualElement m_HeroCardPreviewPanel;
        private ScrollView m_HeroCardPreviewList;
        private ScrollView m_HeroCardPreviewAttrList;


        public HeroCardManagementView(UIDocument document, ILogger logger) : base(document, logger)
        {
        }

        public void Initialize(string name, List<HeroCardSO> data)
        {
            m_TopElement = m_Document.rootVisualElement.Q<VisualElement>(name);
            SetVisualElements();
            RegisterButtonCallbacks();
            InitializeDataViewAndBinding(data);
        }

        private void InitializeDataViewAndBinding(List<HeroCardSO> data)
        {
            m_HeroCardPreviewList =
                m_HeroCardContainer.CreateChildWithName<ScrollView>(k_HeroCardPreviewList);
            m_HeroCardPreviewList.AddToClassList(k_HeroCardPreviewList);
            m_HeroCardPreviewList.pickingMode = PickingMode.Ignore;
            m_HeroCardPreviewList.mode = ScrollViewMode.Horizontal;
            m_HeroCardPreviewList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            m_HeroCardPreviewList.verticalScrollerVisibility = ScrollerVisibility.Auto;
            foreach (var card in data)
            {
                var cardVisual = new HeroCardPreviewElement
                {
                    HeroCardSO = card
                };
                cardVisual.OnClicked += OnPreviewElementClicked;
                m_HeroCardPreviewList.contentContainer.Add(cardVisual);
            }

            m_HeroCardPreviewPanel =
                m_HeroCardContainer.CreateChildWithName<VisualElement>(k_HeroCardPreviewPanel, k_HeroCardPreviewPanel);

            m_HeroCardPreviewAttrList = 
                m_HeroCardPreviewPanel.CreateChildWithName<ScrollView>(k_HeroCardPreviewAttrList, k_HeroCardPreviewAttrList);
            m_HeroCardPreviewAttrList.pickingMode = PickingMode.Ignore;
            m_HeroCardPreviewAttrList.mode = ScrollViewMode.Vertical;
            m_HeroCardPreviewAttrList.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            m_HeroCardPreviewAttrList.verticalScrollerVisibility = ScrollerVisibility.Auto;
        }

        public void BindingPreviewPanel(HeroCardSO heroCard)
        {
            m_HeroCardPreviewAttrList.Clear();
            Type type = heroCard.GetType();
            while (type != null && type != typeof(ScriptableObject))
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                foreach (var field in fields)
                {
                    if (field.GetCustomAttribute<CreatePropertyAttribute>() == null)
                        continue;

                    VisualElement fieldElement = CreateFieldForType(field, heroCard);
                    if (fieldElement == null)
                        continue;

                    m_HeroCardPreviewAttrList.contentContainer.Add(fieldElement);
                }
                type = type.BaseType;
            }
        }
        
        private void OnPreviewElementClicked(HeroCardSO card)
        {
            BindingPreviewPanel(card);
        }
        
        private VisualElement CreateFieldForType(FieldInfo field, HeroCardSO heroCard)
        {
            var fieldType = field.FieldType;
            VisualElement element;

            if (fieldType == typeof(string))
            {
                var textField = new TextField
                {
                    label = field.Name,
                    dataSource = heroCard,
                    multiline = false
                };
                textField.SetBinding("value", new DataBinding
                {
                    dataSourcePath = new PropertyPath(field.Name),
                    bindingMode = BindingMode.TwoWay
                });
                element = textField;
            }
            else if (fieldType == typeof(int) || fieldType == typeof(uint))
            {
                var intField = new IntegerField
                {
                    label = field.Name,
                    dataSource = heroCard
                };
                intField.SetBinding("value", new DataBinding
                {
                    dataSourcePath = new PropertyPath(field.Name),
                    bindingMode = BindingMode.TwoWay
                });
                element = intField;
            }
            else if (fieldType == typeof(float))
            {
                var floatField = new FloatField
                {
                    label = field.Name,
                    dataSource = heroCard
                };
                floatField.SetBinding("value", new DataBinding
                {
                    dataSourcePath = new PropertyPath(field.Name),
                    bindingMode = BindingMode.TwoWay
                });
                element = floatField;
            }
            else
            {
                return null;
            }

            element.AddToClassList(k_HeroCardPreviewAttr);
            return element;
        }

        protected override void SetVisualElements()
        {
            m_HeroCardContainer = m_TopElement.Q<VisualElement>("hero_card_management__container");
        }

        protected override void RegisterButtonCallbacks()
        {
            
        }
    }
}