using CoreDomain.Scripts.Services.Logger;
using CoreDomain.Scripts.Services.UI.Base;
using UnityEngine.UIElements;
using Zenject;

namespace CoreDomain.GameDomain.Scripts.Mvc.Login
{
    public class PlayerInitView : UIView
    {
        private const string k_InitButton = "init__button";
        private const string k_NicknameText = "init__nickname";
        private Button m_InitButton;
        private TextField m_NicknameText;
        [Inject]
        public PlayerInitView(UIDocument document, ILogger logger) : base(document, logger)
        {
        }
        public void Initialize(string name)
        {
            m_TopElement = m_Document.rootVisualElement.Q<VisualElement>(name);
            SetVisualElements();
        }

        protected override void SetVisualElements()
        {
            m_InitButton = m_TopElement.Q<Button>(k_InitButton);
            m_NicknameText = m_TopElement.Q<TextField>(k_NicknameText);
        }
        
        public void SetupCallbacks(EventCallback<ClickEvent> init_callback)
        {
            m_InitButton.RegisterCallback(init_callback);
        }

        public PlayerInitContext GetPlayerInitContext() => new PlayerInitContext(m_NicknameText.text);

        public override void FreezeInterface()
        {
            m_InitButton.SetEnabled(false);
            m_NicknameText.isReadOnly = true;
        }

        public override void UnFreezeInterface()
        {
            m_InitButton.SetEnabled(true);
            m_NicknameText.isReadOnly = false;
        }

        
    }

    public readonly struct PlayerInitContext
    {
        public readonly string nickname;

        public PlayerInitContext(string nickname)
        {
            this.nickname = nickname;
        }
    }
}