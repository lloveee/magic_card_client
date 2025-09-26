using CoreDomain.Scripts.Services.Logger;
using Zenject;

namespace CoreDomain.Scripts.Services.CommandFactory
{
    public class CommandFactory : ICommandFactory
    {
        private readonly DiContainer _container;
        private readonly ILogger _logger;

        public CommandFactory(DiContainer container, ILogger logger)
        {
            _container = container;
            _logger = logger;
        }
        
        public TCommand CreateCommandVoid<TCommand>() where TCommand : ICommandVoid, new()
        {
            TCommand command = new TCommand();
            command.SetObjectResolver(_container, _logger);
            command.ResolverDependencies();
            return command;
        }

        public TCommand CreateCommandWithResult<TCommand, TReturn>() where TCommand : ICommandWithResult<TReturn>, new()
        {
            TCommand command = new TCommand();
            command.SetObjectResolver(_container, _logger);
            command.ResolverDependencies();
            return command;
        }

        public TCommand CreateCommandAsync<TCommand>() where TCommand : ICommandAsync, new()
        {
            TCommand command = new TCommand();
            command.SetObjectResolver(_container, _logger);
            command.ResolverDependencies();
            return command;
        }

        public TCommand CreateCommandAsyncWithResult<TCommand, TReturn>() where TCommand : ICommandAsyncWithResult<TReturn>, new()
        {
            TCommand command = new TCommand();
            command.SetObjectResolver(_container, _logger);
            command.ResolverDependencies();
            return command;
        }
    }
}