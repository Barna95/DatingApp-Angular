namespace API.SignalR
{
    public class PresenceTracker
    {
        //user : connections dictionary
        private static readonly Dictionary<string, List<string>> OnlineUsers = new ();

        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            // using lock on the dictionary to dodge multiple threads using the same dictionary
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string>{connectionId});
                    isOnline = true;
                }
            }
            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

                OnlineUsers[(username)].Remove(connectionId);

                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
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
                onlineUsers = OnlineUsers.OrderBy(k => k.Key)
                    .Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionsForUser(string username)
        {
            //using a dictionary instead of a database is not production ready, but does the job for now
            List<string> connectionsId;
            lock (OnlineUsers)
            {
                connectionsId = OnlineUsers.GetValueOrDefault(username);
            }
            return Task.FromResult(connectionsId);
        }
    }
}
