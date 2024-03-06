using Microsoft.OpenApi.Extensions;
using TopTen.Server.Extensions;

namespace TopTen.Server.Models.ViewModels
{
    public class GroupVM
    {
        public string Id { get; set; }
        public string State { get; set; }
        public string HostId { get; set; }
        public string CaptainId { get; set; }
        public List<PlayerVM> PlayerList { get; set; }
        public int RemainingHp { get; set; }
        public int RoundNumber { get; set; }
        public ThemeVM Theme { get; set; }
        public List<string> SortedAnswers { get; set; }

        public GroupVM(string groupId, Client client,List<Client> clients, Enums.State state, Game game)
        {
            Id = groupId;
            State = state.GetEnumDisplayName();
            HostId = game.HostId;
            CaptainId = game.CaptainId;

            PlayerList = game.PlayerList.Select(p =>
            {
                var isAllowed = new List<Enums.State> { Enums.State.RoundEnded, Enums.State.AnswersSorted }.Contains(state) || p.ClientId == client.Id;
                return new PlayerVM(isAllowed, p, clients.First(c => c.Id == p.ClientId));
            }).ToList();

            RemainingHp = game.RemainingHp;
            RoundNumber = game.RoundNumber;
            var theme = game.ThemeList.FirstOrDefault(theme => theme.Id == game.ThemeId);
            Theme = theme != null ? new ThemeVM(theme) : new ThemeVM();
            SortedAnswers = game.SortedAnswers;
        }
    }
}
