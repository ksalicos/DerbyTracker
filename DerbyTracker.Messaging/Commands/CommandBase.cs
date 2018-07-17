using System;

namespace DerbyTracker.Messaging.Commands
{
    public class CommandBase : ICommand
    {
        public Guid CommandId { get; }
        public string Originator { get; set; }
        public DateTime ServerTime { get; }

        public CommandBase(string originator)
        {
            CommandId = Guid.NewGuid();
            Originator = originator;
            ServerTime = DateTime.Now;
        }
    }
}