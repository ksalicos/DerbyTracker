using DerbyTracker.Common.Exceptions;
using System;
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


        List<NodeConnection> ListConnected();


        //Register node
        //Disconnect node
        //Assign role to node
        //Remove role from node
        bool IsInRole(string nodeId, string role);
        void RemoveRole(string nodeId, string role);
        void AddRole(string nodeId, string role);
        List<string> GetRoles(string nodeId);
    }

    public class NodeConnection
    {
        public string NodeId { get; set; }
        public Guid BoutId { get; set; }
        public string ConnectionId { get; set; }
        public int ConnectionNumber { get; set; }
        public List<string> Roles { get; set; }
    }

    public class NodeService : INodeService
    {
        private readonly Dictionary<string, NodeConnection> _nodeIdToConnectionId = new Dictionary<string, NodeConnection>();

        private readonly IBoutRunnerService _boutRunnerService;

        public NodeService(IBoutRunnerService boutRunnerService)
        {
            _boutRunnerService = boutRunnerService;
        }

        public bool IsConnected(string nodeId)
        {
            return _nodeIdToConnectionId.ContainsKey(nodeId);
        }

        public NodeConnection ConnectNode(string nodeId, string connectionId)
        {
            if (!IsConnected(nodeId))
            {
                //If there is only one bout running, add new nodes to it
                //Otherwise do not assign a bout (Guid.Empty)
                var running = _boutRunnerService.RunningBouts();
                var boutId = running.Count == 1 ? running.Single().BoutId : Guid.Empty;

                _nodeIdToConnectionId[nodeId] = new NodeConnection
                {
                    BoutId = boutId,
                    NodeId = nodeId,
                    ConnectionNumber = NextConnectionNumber(),
                    Roles = new List<string>()
                };
            }

            _nodeIdToConnectionId[nodeId].ConnectionId = connectionId;

            return _nodeIdToConnectionId[nodeId];
        }

        public List<NodeConnection> ListConnected()
        {
            var list = _nodeIdToConnectionId.Select(x => x.Value).ToList();
            return list;
        }

        public bool IsInRole(string nodeId, string role)
        {
            if (!_nodeIdToConnectionId.ContainsKey(nodeId))
            {
                throw new NoSuchNodeException(nodeId);
            }
            return _nodeIdToConnectionId[nodeId].Roles.Contains(role);
        }

        public void RemoveRole(string nodeId, string role)
        {
            if (!_nodeIdToConnectionId.ContainsKey(nodeId))
            {
                throw new NoSuchNodeException(nodeId);
            }
            _nodeIdToConnectionId[nodeId].Roles.Remove(role);
        }

        public void AddRole(string nodeId, string role)
        {
            if (!_nodeIdToConnectionId.ContainsKey(nodeId))
            {
                throw new NoSuchNodeException(nodeId);
            }
            _nodeIdToConnectionId[nodeId].Roles.Add(role);
        }

        public List<string> GetRoles(string nodeId)
        {
            if (!_nodeIdToConnectionId.ContainsKey(nodeId))
            {
                throw new NoSuchNodeException(nodeId);
            }
            return _nodeIdToConnectionId[nodeId].Roles;
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
