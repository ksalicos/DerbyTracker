using System;

namespace DerbyTracker.Common.Exceptions
{
    public class NodeNotAuthorizedException : Exception
    {
        public string NodeId { get; set; }
        public string CommandName { get; set; }

        public NodeNotAuthorizedException(string nodeId, string commandName)
            : base($"Node [{nodeId}] attempted to run command [{commandName}], but was not authorized")
        {
            NodeId = nodeId;
        }
    }

}
