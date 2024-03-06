namespace TopTen.Server.Models
{
    public class Player
    {
        public string ClientId { get; set; }
        public int Weight { get; set; }
        public string Answer { get; set; }
        public bool AnswerLocked { get; set; }

        public Player(string id, string name, string avatar)
        {
            ClientId=id;
            Weight=-1;
            Answer=string.Empty;
            AnswerLocked=false;
        }
    }
}
