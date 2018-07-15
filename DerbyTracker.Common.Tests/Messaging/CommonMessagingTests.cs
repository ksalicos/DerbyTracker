using DerbyTracker.Messaging.Commands;
using DerbyTracker.Messaging.Handlers;
using System;
using System.Linq;
using Xunit;

namespace DerbyTracker.Common.Tests.Messaging
{
    public class CommonMessagingTests
    {
        [Fact]
        public void AllCommandsHaveHandlers()
        {
            var commonAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.FullName.StartsWith("DerbyTracker.Common") && !x.FullName.Contains("Test"));

            var commandType = typeof(ICommand);
            var handlerType = typeof(ICommandHandler);

            var types = commonAssembly.GetTypes()
                .Where(p => commandType.IsAssignableFrom(p) && !p.IsAbstract);

            foreach (var type in types)
            {
                var handlers = commonAssembly.GetTypes()
                    .Where(p => handlerType.IsAssignableFrom(p) && !p.IsAbstract);
                Assert.Contains(handlers, x => x.Name.Contains($"{type.Name}Handler"));
                //TODO: Check for HandlesAttribute?
            }
        }
    }
}
