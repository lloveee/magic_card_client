using CoreDomain.Scripts.Services.UI.Base;
using Unity.Properties;
using UnityEngine.UIElements;
using Zenject;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home
{
    public class HomeScreenView : UIView
    {
        private const string k_RankIcon = "home__body_rank_icon";
        private const string k_RankText = "rank";
        private const string k_NameText = "name";
        private const string k_Container = "home__body_rank_container";
        private const string k_StartMatchBtn = "home__body_btn_startmatch";
        private const string k_CancelMatchBtn = "home__body_btn_cancelmatch";
        private const string k_PracticeRoomBtn = "home__body_practice_btn";

        private VisualElement m_RankIcon;
        private VisualElement m_Container;
        private Label m_RankText;
        private Label m_NameText;
        private Button m_StartMatchBtn;
        private Button m_CancelMatchBtn;
        private Button m_PracticeRoomBtn;
        
        [Inject]
        public HomeScreenView(UIDocument document, ILogger logger) : base(document, logger)
        {
        }
        
        public void Initialize(string name)
        {
            m_TopElement = m_Document.rootVisualElement.Q<VisualElement>(name);
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        public void SetDataBinding(HomeScreenData data)
        {
            
            m_Container.dataSource = data;
            
            m_RankIcon.SetBinding("style.backgroundImage", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(HomeScreenData.RankIcon)),
                bindingMode = BindingMode.ToTarget
            });
            
            m_RankText.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(HomeScreenData.PlayerRank)),
                bindingMode = BindingMode.ToTarget
            });
            
            m_NameText.SetBinding("text", new DataBinding
            {
                dataSourcePath = new PropertyPath(nameof(HomeScreenData.PlayerName)),
                bindingMode = BindingMode.ToTarget
            });
            /*
            m_RankIcon.style.backgroundImage = new StyleBackground(data.RankIcon);
            m_RankText.text = data.PlayerRank;
            m_NameText.text = data.PlayerName;
            */
        }

        protected override void SetVisualElements()
        {
            m_Container = m_TopElement.Q<VisualElement>(k_Container);
            m_RankIcon = m_TopElement.Q<VisualElement>(k_RankIcon);
            m_RankText = m_TopElement.Q<Label>(k_RankText);
            m_NameText = m_TopElement.Q<Label>(k_NameText);
            m_StartMatchBtn = m_TopElement.Q<Button>(k_StartMatchBtn);
            m_CancelMatchBtn = m_TopElement.Q<Button>(k_CancelMatchBtn);
            m_PracticeRoomBtn = m_TopElement.Q<Button>(k_PracticeRoomBtn);
        }
        
        public void SetupCallbacks(
            EventCallback<ClickEvent> start_match_callback, 
            EventCallback<ClickEvent> cancel_match_callback,
            EventCallback<ClickEvent> practice_callback)
        {
            m_StartMatchBtn.RegisterCallback(start_match_callback);
            m_CancelMatchBtn.RegisterCallback(cancel_match_callback);
            m_PracticeRoomBtn.RegisterCallback(practice_callback);
        }

        protected override void RegisterButtonCallbacks()
        {
        }
    }
}