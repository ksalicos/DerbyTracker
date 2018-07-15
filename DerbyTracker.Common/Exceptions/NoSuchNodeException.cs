using System;

namespace DerbyTracker.Common.Exceptions
{
    public class NoSuchNodeException : Exception
    {
        public string NodeId { get; set; }

        public NoSuchNodeException(string nodeId) : base($"Attempt was made to access node with id: [{nodeId}], but none was connected")
        {
            NodeId = nodeId;
        }
    }

}
