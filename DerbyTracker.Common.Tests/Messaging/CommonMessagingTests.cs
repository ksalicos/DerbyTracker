using DerbyTracker.Common.Messaging.CommandHandlers;
using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging
{
    public class CommonMessagingTests
    {
        private readonly IEnumerable<Type> _allHandlers;
        private readonly Assembly _commonAssembly;

        public CommonMessagingTests()
        {
            _commonAssembly = typeof(HandlerRegistrar).Assembly;

            var handlerType = typeof(ICommandHandler);
            _allHandlers = _commonAssembly.GetTypes()
                .Where(p => handlerType.IsAssignableFrom(p) && !p.IsAbstract);
        }

        [Fact]
        public void AllCommandsHaveHandlers()
        {
            var commandType = typeof(ICommand);
            var commands = _commonAssembly.GetTypes()
                .Where(p => commandType.IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var type in commands)
            {
                Assert.Contains(_allHandlers, x => x.Name.Contains($"{type.Name}Handler"));
                //TODO: Check for HandlesAttribute?
            }
        }

        [Fact]
        public void AllHandlersHaveOnlyOneConstructor()
        {
            Assert.True(_allHandlers.All(x => x.GetConstructors().Count() == 1));
        }

        [Fact]
        public void AllHandlersHaveHandlesAttribute()
        {
            Assert.True(_allHandlers.All(handler =>
                Attribute.GetCustomAttribute(handler, typeof(HandlesAttribute)) != null
            ));
        }
    }
}
