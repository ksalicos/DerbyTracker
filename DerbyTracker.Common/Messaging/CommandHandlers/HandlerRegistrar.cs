using DerbyTracker.Messaging.Dispatchers;
using DerbyTracker.Messaging.Handlers;
using System;
using System.Linq;

namespace DerbyTracker.Common.Messaging.CommandHandlers
{
    public class HandlerRegistrar
    {
        private readonly IServiceProvider _serviceProvider;

        public HandlerRegistrar(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void RegisterHandlers(ImmediateDispatcher dispatcher)
        {
            var assembly = typeof(HandlerRegistrar).Assembly;
            var handlerType = typeof(ICommandHandler);
            var handlers = assembly.GetTypes()
                .Where(p => handlerType.IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var handler in handlers)
            {
                var constructor = handler.GetConstructors().Single();
                var parameters = constructor.GetParameters();
                var instances = parameters.Select(p => _serviceProvider.GetService(p.ParameterType)).ToArray();
                var newHandler = Activator.CreateInstance(handler, instances) as ICommandHandler;
                dispatcher.RegisterHandler(newHandler);
            }
        }
    }
}
