namespace CoreDomain.GameDomain.Scripts.Mvc.Login
{
    public interface ILoginController
    {
        public void Login(string name, string pwd);
        public void Initialize();
        public void FreezeInterface();
        public void UnfreezeInterface();
        public void HideView();
        public void ShowView();
    }
}