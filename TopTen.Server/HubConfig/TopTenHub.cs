using Microsoft.AspNetCore.SignalR;
using TopTen.Server.Manager;
using TopTen.Server.Models;
using TopTen.Server.Models.Request;

namespace TopTen.Server.HubConfig
{
    public class TopTenHub : Hub
    {

        private readonly GroupManager _groupManager;
        private readonly ClientManager _clientManager;

        public TopTenHub(GroupManager group)
        {
            _groupManager = group;
        }

        public override async Task OnConnectedAsync()
        {
            var id = Context.GetHttpContext().Request.Query["id"];
            Client client;

            if (string.IsNullOrEmpty(id))
            {
                client = _clientManager.AddClient(Context.ConnectionId);
                await Clients.Caller.SendAsync("Connected", client.Id);
                return;
            }
                
            client = _clientManager.GetClientById(id);

            if(client == null) 
            {
                await SendErrorMessage("Impossible to connect");
                return;
            }

            await Clients.Caller.SendAsync("Connected", client.Id);

            if(client.ConnectionId != Context.ConnectionId)
            {
                _clientManager.SetClientConnectionId(client.Id, Context.ConnectionId);
            }

            if (!string.IsNullOrEmpty(client.GroupId))
            {
                if (_groupManager.DoesGroupExist(client.GroupId))
                {
                    await Groups.AddToGroupAsync(client.ConnectionId, client.GroupId);
                    await SendUpdatedMessage(client.GroupId);
                }
                else
                {
                    _clientManager.SetClientGroupId(client.Id, string.Empty);
                }
            }
        }

        public async Task CreateGroup(CreateGroupRequest req)
        {
            try
            {
                string connectionId = Context.ConnectionId;
                var client = _clientManager.GetClientByConnectionId(connectionId);

                if(client == null)
                {
                    await SendErrorMessage("This user does not exist");
                    return;
                }
                // Générer un ID de groupe
                string groupId;

                do
                {
                    groupId = Guid.NewGuid().ToString().Substring(0, 6);
                }
                while (_groupManager.DoesGroupExist(groupId));

                
                // Créer le groupe dans le GroupManager
                if (_groupManager.CreateGroup(groupId, connectionId, req.Name, req.Avatar))
                {
                    client.GroupId = groupId;
                    client.Name = req.Name;
                    client.Avatar = req.Avatar;
                    _clientManager.SetClientGroupId(client.Id, client.GroupId);
                    _clientManager.SetClientName(client.Id, client.Name);
                    _clientManager.SetClientAvatar(client.Id, client.Avatar);
                    // Ajouter l'utilisateur au groupe SignalR
                    await Groups.AddToGroupAsync(connectionId, groupId);
                    await SendUpdatedMessage(groupId);
                }
                else
                {
                    await SendErrorMessage("Impossible to create a group");
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task JoinGroup(JoinGroupRequest req)
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(req.Group))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if(_groupManager.IsPlayerPartOfGroup(req.Group, connectionId))
            {
                await SendErrorMessage("This client is already part of this group");
                return;
            }

            // Ajouter l'utilisateur au groupe dans le GroupManager
            if(_groupManager.JoinGroup(req.Group, connectionId, req.Name, req.Avatar))
            {
                client.GroupId = req.Group;
                client.Name = req.Name;
                client.Avatar = req.Avatar;
                _clientManager.SetClientGroupId(client.Id, client.GroupId);
                _clientManager.SetClientName(client.Id, client.Name);
                _clientManager.SetClientAvatar(client.Id, client.Avatar);
                // Ajouter l'utilisateur au groupe SignalR
                await Groups.AddToGroupAsync(connectionId, req.Group);
                await SendUpdatedMessage(req.Group);
            }
            else
            {
                await SendErrorMessage("Impossible to join this group");
            }
        }

        public async Task LeaveGroup(string groupId)
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(groupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if(!_groupManager.IsPlayerPartOfGroup(groupId, connectionId)){
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if(_groupManager.LeaveGroup(groupId, connectionId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, groupId);
                await SendUpdatedMessage(groupId);
            }
        }

        public async Task StartGame()
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientById(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This client does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (!_groupManager.IsPlayerHost(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not allowed to make this action");
            }

            if (_groupManager.GetGroupState(client.GroupId) != Models.Enums.State.GameReady)
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            _groupManager.StartGame(client.GroupId);
            await SendUpdatedMessage(client.GroupId);
        }

        public async Task StartRound()
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This client does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (!_groupManager.IsPlayerCaptain(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not allowed to make this action");
                return;
            }

            if (_groupManager.GetGroupState(client.GroupId) != Models.Enums.State.RoundReady)
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            //Passage en StartRound
            _groupManager.StartRound(client.GroupId);

            //Envoyer l'état
            await SendUpdatedMessage(client.GroupId);
        }

        public async Task LockAnswer(string answer)
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (_groupManager.GetGroupState(client.GroupId) != Models.Enums.State.RoundRunning)
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            await _groupManager.LockAnswer(client.GroupId, connectionId, answer);

            //Envoyer l'état
            await SendUpdatedMessage(client.GroupId);
        }

        public async Task UnlockAnswer()
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (_groupManager.GetGroupState(client.GroupId) != Models.Enums.State.RoundRunning)
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            await _groupManager.UnlockAnswer(client.GroupId, connectionId);

            //Envoyer l'état
            await SendUpdatedMessage(client.GroupId); 
        }

        public async Task SortAnswers(List<string> sortedAnswers)
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (!_groupManager.IsPlayerCaptain(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not allowed to make this action");
                return;
            }

            if (_groupManager.GetGroupState(client.GroupId) != Models.Enums.State.RoundEnded)
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            await _groupManager.SortAnswer(client.GroupId, sortedAnswers);

            //Envoyer les SortedAnswers
            await SendUpdatedMessage(client.GroupId);
        }

        public async Task LockSortedAnswers()
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (!_groupManager.IsPlayerCaptain(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not allowed to make this action");
                return;
            }

            if (_groupManager.GetGroupState(client.GroupId) != Models.Enums.State.RoundEnded)
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            await _groupManager.LockSortedAnswer(client.GroupId);

            //Envoyer les SortedAnswers
            await SendUpdatedMessage(client.GroupId);
        }

        public async Task ReturnToLobby()
        {
            string connectionId = Context.ConnectionId;
            var client = _clientManager.GetClientByConnectionId(connectionId);

            if (client == null)
            {
                await SendErrorMessage("This user does not exist");
                return;
            }

            if (!_groupManager.DoesGroupExist(client.GroupId))
            {
                await SendErrorMessage("This group does not exist");
                return;
            }

            if (!_groupManager.IsPlayerPartOfGroup(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not part of this group");
                return;
            }

            if (!_groupManager.IsPlayerHost(client.GroupId, connectionId))
            {
                await SendErrorMessage("This client is not allowed to make this action");
                return;
            }

            if (!new List<Models.Enums.State> { Models.Enums.State.Victory, Models.Enums.State.Defeat }.Contains(_groupManager.GetGroupState(client.GroupId)))
            {
                await SendErrorMessage("This game is not in the right state to allow this action");
                return;
            }

            await _groupManager.ReturnToLobby(client.GroupId);
            await SendUpdatedMessage(client.GroupId);
        }

        private async Task SendUpdatedMessage(string groupId)
        {
            try
            {
                if(!_groupManager.DoesGroupExist(groupId))
                {
                    await SendErrorMessage("This group does not exist");
                    return;
                }

                var clients = _groupManager.GetPlayers(groupId).Select(_clientManager.GetClientById).ToList();

                if(clients.Any())
                {
                    var vms = _groupManager.GetGroupVMs(groupId, clients);
                    var tasks = vms.Select(v => Clients.Client(v.Key).SendAsync("Updated", v.Value));
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SendErrorMessage(string message)
        {
            try
            {
                await Clients.Caller.SendAsync("Error", message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
