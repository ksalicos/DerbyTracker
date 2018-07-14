using System.Collections.Generic;
using System.Linq;

namespace DerbyTracker.Common.Services
{
    public interface INodeService
    {
        /// <summary>
        /// Checks to see if node is already connected
        /// </summary>
        /// <param name="nodeId">Id of node to check</param>
        /// <returns>True if node connected, false otherwise.</returns>
        bool IsConnected(string nodeId);

        /// <summary>
        /// Connect node to master
        /// </summary>
        /// <param name="nodeId">Node to connect</param>
        /// <param name="connectionId">Id node is connecting from</param>
        /// <returns>Connection data for node</returns>
        NodeConnection ConnectNode(string nodeId, string connectionId);

        //Register node
        //Disconnect node
        //Assign role to node
        //Remove role from node
    }

    public class NodeConnection
    {
        public string ConnectionId { get; set; }
        public int ConnectionNumber { get; set; }
        public List<string> NodeRoles { get; set; }
    }

    public class NodeService : INodeService
    {
        private readonly Dictionary<string, NodeConnection> _nodeIdToConnectionId = new Dictionary<string, NodeConnection>();

        public bool IsConnected(string nodeId)
        {
            return _nodeIdToConnectionId.ContainsKey(nodeId);
        }

        public NodeConnection ConnectNode(string nodeId, string connectionId)
        {
            if (!IsConnected(nodeId))
            {
                _nodeIdToConnectionId[nodeId] = new NodeConnection
                {
                    ConnectionNumber = NextConnectionNumber(),
                    NodeRoles = new List<string>()
                };
            }

            _nodeIdToConnectionId[nodeId].ConnectionId = connectionId;

            return _nodeIdToConnectionId[nodeId];
        }

        private int NextConnectionNumber()
        {
            var i = 1;
            while (_nodeIdToConnectionId.Any(x => x.Value.ConnectionNumber == i))
            {
                i++;
            }
            return i;
        }
    }
}
