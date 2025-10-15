namespace CoreDomain.Scripts.Mvc.Loading
{
    public class LoadingController : ILoadingController
    {
        private readonly LoadingView m_LoadingView;
        public LoadingController(LoadingView loadingView)
        {
            m_LoadingView = loadingView;
        }
        
        public void Initialize() => m_LoadingView.Initialize();

        public void SetProgress(float progress) => m_LoadingView.SetProgressValue(progress);

        public void SetInfo(string info) => m_LoadingView.SetInfo(info);

        public void Show() => m_LoadingView.Show();
        public void Hide() => m_LoadingView.Hide();
    }
}