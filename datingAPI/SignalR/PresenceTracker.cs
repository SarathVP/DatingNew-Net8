namespace datingAPI.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = [];

        public Task<bool> UserConnected(string username, string connectionId)
        {
            var isOnline = false;
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, [connectionId]);
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string useranme, string connectionId)
        {
            var isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(useranme)) return Task.FromResult(isOffline);

                OnlineUsers[useranme].Remove(connectionId);

                if (OnlineUsers[useranme].Count == 0)
                {
                    OnlineUsers.Remove(useranme);
                    isOffline = true;
                }
            }
            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = [.. OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key)];
            }

            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;

            if (OnlineUsers.TryGetValue(username, out var connections))
            {
                lock (connections)
                {
                    connectionIds = [.. connections];
                }

            }
            else
            {
                connectionIds = [];
            }
            return Task.FromResult(connectionIds);  
        }
    }
}