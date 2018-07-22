using System;

namespace DerbyTracker.Common.Messaging
{
    public class Audiences
    {
        public const string All = "ALL";
        public const string Master = "MASTER";
        public const string Nodes = "NODES";
        public static string Bout(Guid boutId) => $"Bout:{boutId}";
    }
}
