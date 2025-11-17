using CoreDomain.Scripts.Services.UI.Base;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UIElements.Button;
using ILogger = CoreDomain.Scripts.Services.Logger.ILogger;

namespace CoreDomain.GameDomain.Scripts.Mvc.Login
{
    public class LoginView : UIView
    {
        private const string k_LoginButtonInactivateClass = "login__button";
        private const string k_LoginButtonActivateClass = "login__button--active";
        private const string k_LoginButton = "login__button";
        private const string k_UsernameText = "login__userbar";
        private const string k_PasswordText = "login__pwdbar";
        private const string k_AutoLoginToggle = "login__auto_login";
        private Button m_LoginButton;
        private TextField m_UsernameText;
        private TextField m_PasswordText;
        private Toggle m_AutoLoginToggle;
        //TODO should make a controller, register button callback
        
        [Inject]
        public LoginView(UIDocument topElement, ILogger logger) : base(topElement, logger)
        {
            
        }

        public void Initialize(string name)
        {
            m_TopElement = m_Document.rootVisualElement.Q<VisualElement>(name);
            SetVisualElements();
            RegisterButtonCallbacks();
            Hide();
        }

        protected override void SetVisualElements()
        {
            m_LoginButton = m_TopElement.Q<Button>(k_LoginButton);
            m_UsernameText = m_TopElement.Q<TextField>(k_UsernameText);
            m_PasswordText = m_TopElement.Q<TextField>(k_PasswordText);
            m_AutoLoginToggle = m_TopElement.Q<Toggle>(k_AutoLoginToggle);
        }

        protected override void RegisterButtonCallbacks()
        {
        }

        public void SetupCallbacks(EventCallback<ClickEvent> login_callback)
        {
            m_LoginButton.RegisterCallback(login_callback);
        }

        public LoginContext GetLoginContext()
        {
            return new LoginContext(m_UsernameText.text, m_PasswordText.text);
        }

        public void SetLoginContext(string username, string password)
        {
            m_UsernameText.value = username;
            m_PasswordText.value = password;
        }

        public bool GetAutoLogin()
        {
            return m_AutoLoginToggle.value;
        }

        public void SetAutoLogin(bool value)
        {
            m_AutoLoginToggle.value = value;
        }

        public override void FreezeInterface()
        {
            m_LoginButton.SetEnabled(false);
            m_UsernameText.isReadOnly = true;
            m_PasswordText.isReadOnly = true;
        }

        public override void UnFreezeInterface()
        {
            m_LoginButton.SetEnabled(true);
            m_UsernameText.isReadOnly = false;
            m_PasswordText.isReadOnly = false;
        }

        private void OnClickLoginButton(ClickEvent evt)
        {
            //m_logger.Log("LoginButtonClicked");
        }
    }

    public readonly struct LoginContext
    {
        public readonly string username;
        public readonly string password;

        public LoginContext(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}