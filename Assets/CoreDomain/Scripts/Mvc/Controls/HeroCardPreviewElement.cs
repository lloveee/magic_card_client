using System;
using CoreDomain.GameDomain.GameStateDomain.GamePlayDomain.Scripts.GamePlayData.HeroCardData.HeroData;
using CoreDomain.Scripts.Extensions;
using Unity.Properties;
using UnityEngine.UIElements;

namespace CoreDomain.Scripts.Mvc.Controls
{
    [UxmlElement]
    public partial class HeroCardPreviewElement : VisualElement
    {
        static class ClassNames
        {
            public static string HeroCardPreviewBackground = "hero_card_pre__background";
            public static string HeroCardPreviewImage = "hero_card_pre__image";
            public static string HeroCardPreviewName = "hero_card_pre__name";
            public static string HeroCardPreviewItem = "hero_card_pre__item";
        }
        
        public event Action<HeroCardSO> OnClicked;
        readonly Label m_HeroNameLabel;
        readonly VisualElement m_Background;
        readonly VisualElement m_HeroImage;

        [CreateProperty]
        public HeroCardSO HeroCardSO
        {
            get => (HeroCardSO)dataSource;
            set => dataSource = value;
        }

        public HeroCardPreviewElement()
        {
            AddToClassList(ClassNames.HeroCardPreviewItem);
            m_Background = new VisualElement { name = ClassNames.HeroCardPreviewBackground };
            m_Background.AddToClassList(ClassNames.HeroCardPreviewBackground);
            Add(m_Background);

            m_HeroImage = m_Background.CreateChildWithName(ClassNames.HeroCardPreviewImage, ClassNames.HeroCardPreviewImage);

            m_HeroNameLabel = m_Background.CreateChildWithName<Label>(ClassNames.HeroCardPreviewName, ClassNames.HeroCardPreviewName);
            
            BindElements();
            
            RegisterCallback<ClickEvent>(OnClick);
        }

        void BindElements()
        {
            m_HeroNameLabel.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(HeroCardSO.cardName)),
                bindingMode = BindingMode.ToTarget
            });
            
            //Image
        }
        
        private void OnClick(ClickEvent evt)
        {
            OnClicked?.Invoke(HeroCardSO);
        }
    }
}