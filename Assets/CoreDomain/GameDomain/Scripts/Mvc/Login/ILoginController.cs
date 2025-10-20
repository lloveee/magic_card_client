namespace CoreDomain.GameDomain.Scripts.Mvc.Login
{
    public interface ILoginController
    {
        public void Initialize();
        public void FreezeInterface();
        public void UnfreezeInterface();
        public void HideView();
        public void ShowView();
    }
}