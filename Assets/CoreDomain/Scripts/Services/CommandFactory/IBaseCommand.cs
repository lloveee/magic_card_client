using CoreDomain.Scripts.Services.Logger;
using Zenject;

namespace CoreDomain.Scripts.Services.CommandFactory
{
    public interface IBaseCommand
    {
        void SetObjectResolver(DiContainer container, ILogger logger);
        void ResolverDependencies();
    }
}