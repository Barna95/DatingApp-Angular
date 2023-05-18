namespace API.Entities
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }

        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        // to satisfy EF when we create the DB, we need to give this an empty constructor, since we can't expect the connection infos at start
        public Connection()
        {
            
        }
    }
}
