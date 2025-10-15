namespace CoreDomain.Scripts.Mvc.Loading
{
    public interface ILoadingController
    {
        public void Initialize();
        public void SetProgress(float progress);
        public void SetInfo(string info);
        public void Show();
        public void Hide();
    }
}