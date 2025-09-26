using CoreDomain.Scripts.Services.Logger;
using Zenject;

namespace CoreDomain.Scripts.Services.CommandFactory
{
    public abstract class BaseCommand : IBaseCommand
    {
        protected DiContainer _container;
        protected ILogger _logger;
        public void SetObjectResolver(DiContainer container, ILogger logger)
        {
            _container = container;
            _logger = logger;
        }

        public abstract void ResolverDependencies();
    }
}