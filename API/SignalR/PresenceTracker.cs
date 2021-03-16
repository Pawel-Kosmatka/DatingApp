using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static Dictionary<string, List<string>> onlineUsers = new Dictionary<string, List<string>>();

        public Task<bool> UserConnected(string userName, string connectionId)
        {
            bool isOnline = false;
            lock (onlineUsers)
            {
                if (onlineUsers.ContainsKey(userName))
                {
                    onlineUsers[userName].Add(connectionId);
                }
                else
                {
                    onlineUsers.Add(userName, new List<string> { connectionId });
                    isOnline = true;
                }
            }
            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string userName, string connectionId)
        {
            bool isOffline = false;
            lock (onlineUsers)
            {
                if (!onlineUsers.ContainsKey(userName)) return Task.FromResult(isOffline);

                onlineUsers[userName].Remove(connectionId);
                if (onlineUsers[userName].Count == 0)
                {
                    onlineUsers.Remove(userName);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] usersCurrentlyOnline;
            lock (onlineUsers)
            {
                usersCurrentlyOnline = onlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(usersCurrentlyOnline);
        }

        public Task<List<string>> GetConnectionsForUser(string userName)
        {
            List<string> connectionIds;
            lock (onlineUsers)
            {
                connectionIds = onlineUsers.GetValueOrDefault(userName);
            }

            return Task.FromResult(connectionIds);
        }
    }
}