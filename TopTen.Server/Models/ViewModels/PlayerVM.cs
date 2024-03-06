namespace TopTen.Server.Models.ViewModels
{
    public class PlayerVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Weight { get; set; }
        public string? Answer { get; set; }
        public bool AnswerLocked { get; set; }

        public PlayerVM(bool isAllowed, Player player, Client client) 
        {
            Id = player.ClientId;
            Name = client.Name;
            Avatar = client.Avatar;
            Weight = player.Weight;
            Answer = isAllowed ? player.Answer : default(string?);
            AnswerLocked = player.AnswerLocked;
        }
    }
}
