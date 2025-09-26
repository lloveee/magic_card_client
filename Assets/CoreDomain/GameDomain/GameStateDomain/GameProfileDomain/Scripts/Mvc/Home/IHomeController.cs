using SpacetimeDB.Types;

namespace CoreDomain.GameDomain.GameStateDomain.GameProfileDomain.Scripts.Mvc.Home
{
    public interface IHomeController
    {
        void Initialize();
        void InitHomeData(PlayerAccount data);
        void ShowView();
        void HideView();
    }
}