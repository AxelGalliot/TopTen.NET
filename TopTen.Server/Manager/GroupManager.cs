using TopTen.Server.Models;
using TopTen.Server.Models.ViewModels;

namespace TopTen.Server.Manager
{
    public class GroupManager
    {
        private Dictionary<string, TopTen.Server.FiniteStateMachine.FiniteStateMachine> _groups = new Dictionary<string, TopTen.Server.FiniteStateMachine.FiniteStateMachine>();
        private readonly ClientManager _clientManager;
        private TimeSpan _inactivityTimeout = TimeSpan.FromMinutes(30);

        public bool DoesGroupExist(string groupId)
        {
            return _groups.ContainsKey(groupId);
        }

        public bool IsPlayerHost(string groupId, string playerId)
        {
            return _groups[groupId].game.HostId == playerId;
        }

        public bool IsPlayerCaptain(string groupId, string playerId)
        {
            return _groups[groupId].game.CaptainId == playerId;
        }

        public bool IsPlayerPartOfGroup(string groupId, string playerId)
        {
            return _groups[groupId].game.PlayerList.Any(player => player.ClientId == playerId);
        }

        public bool CreateGroup(string groupId, string playerId, string playerName, string playerAvatar)
        {
            if (!_groups.ContainsKey(groupId))
            {
                var fsm = new TopTen.Server.FiniteStateMachine.FiniteStateMachine(new Player(playerId, playerName, playerAvatar));
                _groups[groupId] = fsm;
                StartMonitoringInactivity(groupId);

                return true;
            }

            return false;
        }

        public bool RemoveGroup(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                _groups.Remove(groupId);

                return true;
            }

            return false;
        }

        public bool JoinGroup(string groupId, string playerId, string playerName, string playerAvatar)
        {
            if (_groups.ContainsKey(groupId))
            {
                // Update last activity time
                _groups[groupId].LastActivityTime = DateTime.UtcNow;
                _groups[groupId].game.AddPlayer(new Player(playerId, playerName, playerAvatar));

                return true;
            }

            return false;
        }

        public bool LeaveGroup(string groupId, string playerId)
        {
            if (_groups.ContainsKey(groupId))
            {
                _groups[groupId].game.RemovePlayer(playerId);
                if (!_groups[groupId].game.PlayerList.Any())
                    _groups.Remove(groupId);

                return true;
            }

            return false;
        }

        public Enums.State GetGroupState(string groupId)
        {
            return _groups[groupId].GetMachineState();
        }

        public Dictionary<string, GroupVM> GetGroupVMs(string groupId, List<Client> clients)
        {
            if (_groups.ContainsKey(groupId))
            {
                var groups = clients.Select(c => new KeyValuePair<string, GroupVM>(c.ConnectionId, new GroupVM(groupId, c, clients, _groups[groupId].GetMachineState(), _groups[groupId].game))).ToDictionary();
                return groups;
            }

            return null;
        }

        public List<string> GetPlayers(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                return _groups[groupId].game.PlayerList.Select(p => p.ClientId).ToList();
            }

            return null;
        }

        public async Task StartGame(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                // Trigger game start event on the state machine
                await _groups[groupId].StartGameAsync();
            }
        }

        public async Task StartRound(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                // Trigger game start event on the state machine
                await _groups[groupId].StartRoundAsync();
            }
        }

        public async Task LockAnswer(string groupId, string playerId, string answer)
        {
            if (_groups.ContainsKey(groupId))
            {
                var player = _groups[groupId].game.PlayerList.First(player => player.ClientId == playerId);

                if (player != null)
                {
                    player.Answer = answer;
                    player.AnswerLocked = true;

                    await _groups[groupId].ReceiveAnswersAsync();
                }
            }
        }

        public async Task UnlockAnswer(string groupId, string playerId)
        {
            if (_groups.ContainsKey(groupId))
            {
                var player = _groups[groupId].game.PlayerList.First(player => player.ClientId == playerId);

                if (player != null)
                {
                    player.AnswerLocked = false;

                    await _groups[groupId].ReceiveAnswersAsync();
                }
            }
        }

        public async Task SortAnswer(string groupId, List<string> sortedAnswers)
        {
            if (_groups.ContainsKey(groupId))
            {
                _groups[groupId].game.SortedAnswers = sortedAnswers;
            }
        }

        public async Task LockSortedAnswer(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                await _groups[groupId].CaptainGuessedAsync();

                //Envoyer les HP restants
                if (_groups[groupId].game.RemainingHp == 0)
                    await _groups[groupId].NoHpLeftAsync();

                else if (_groups[groupId].game.RoundNumber >= 5)
                    await _groups[groupId].AllRoundDoneAsync();

                else
                    await _groups[groupId].RoundAndHpOkAsync();
            }
        }

        public async Task ReturnToLobby(string groupId)
        {
            if (_groups.ContainsKey(groupId))
            {
                await _groups[groupId].ReturnToLobbyAsync();
            }
        }

        private void StartMonitoringInactivity(string groupId)
        {
            // Start monitoring inactivity for the group
            // If inactivity exceeds the timeout, trigger inactivity timeout event on the state machine
            if (_groups.ContainsKey(groupId))
            {
                // Start a timer to monitor inactivity for the group
                Timer timer = new Timer(_ =>
                {
                    // Check if the group is still active
                    if (_groups.ContainsKey(groupId))
                    {
                        // Calculate the time elapsed since the last activity
                        TimeSpan elapsedTime = DateTime.UtcNow - _groups[groupId].LastActivityTime;

                        // Check if the group has been inactive for the timeout duration
                        if (elapsedTime >= _inactivityTimeout)
                        {
                            // Trigger inactivity timeout event on the state machine
                            RemoveGroup(groupId);
                        }
                    }
                }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Check for inactivity every minute
            }
        }
    }
}
