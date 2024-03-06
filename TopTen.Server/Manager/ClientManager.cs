using TopTen.Server.Models;

namespace TopTen.Server.Manager
{
    public class ClientManager
    {
        private List<Client> _clients = new List<Client>();

        public Client? GetClientById(string id)
        {
            return _clients.FirstOrDefault(c => c.Id == id);
        }

        public Client? GetClientByConnectionId(string connectionId)
        {
            return _clients.FirstOrDefault(c => c.Id == connectionId);
        }

        public Client AddClient(string connectionId)
        {
            var id = new Guid();
            _clients.Add(new Client { Id = id.ToString(), ConnectionId = connectionId });

            return _clients.First(c => c.ConnectionId == connectionId);
        }

        public bool RemoveClient(string id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
                return false;

            _clients.Remove(client);
            return true;
        }

        public bool SetClientConnectionId(string id,  string connectionId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if(client == null)
                return false;

            client.ConnectionId = connectionId;
            return true;
        }

        public bool SetClientGroupId(string id,  string groupId)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if(client == null)
                return false;

            client.GroupId = groupId;
            return true;
        }

        public bool SetClientName(string id,  string name)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if(client == null)
                return false;

            client.Name = name;
            return true;
        }

        public bool SetClientAvatar(string id,  string avatar)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if(client == null)
                return false;

            client.Avatar = avatar;
            return true;
        }
    }
}
