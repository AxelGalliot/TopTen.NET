using Newtonsoft.Json;
using System.Net.Http.Json;

namespace TopTen.Server.Models
{
    public class Game
    {
        public int RemainingHp { get; set; }
        public int RoundNumber { get; set; }
        public List<Player> PlayerList { get; set; }
        public string HostId { get; set; }
        public string CaptainId { get; set; }
        public List<string> CaptainList { get; set; }
        public int ThemeId { get; set; }
        public List<Theme> ThemeList { get; set; }
        public List<string> SortedAnswers { get; set; }

        private Random random = new Random();

        public Game(string host, Player player)
        {
            RemainingHp = -1;
            RoundNumber = -1;
            PlayerList = new List<Player> { player };
            HostId = host;
            CaptainId = string.Empty;
            CaptainList = new List<string>();
            ThemeId = -1;
            ThemeList = new List<Theme>();
            SortedAnswers = new List<string>();
        }

        public void AddPlayer(Player player)
        {
            PlayerList.Add(player);
        }

        public void RemovePlayer(string _player)
        {
            PlayerList.Remove(PlayerList.First(player => player.ClientId == _player));
        }

        public void InitializeGame()
        {
            RemainingHp = PlayerList.Count;
            RoundNumber = 0;
            PickThemes();
            PickCaptains();
            InitializeRound();
        }

        public void InitializeRound()
        {
            RoundNumber += 1;
            PickCaptain();
            PickTheme();
            DealWeights();
            SortedAnswers = new List<string>();
        }

        public void PickCaptains()
        {
            var players = PlayerList.Select(x => x).ToList();
            var playerCount = players.Count;
            var captainList = new List<string>();

            for(int i = 0; i <= 4; i++)
            {
                if(playerCount < i + 1)
                {
                    captainList.Add(captainList[i - playerCount]);
                }
                else
                {
                    int randomIndex = random.Next(players.Count);
                    captainList.Add(players[randomIndex].ClientId);
                    players.RemoveAt(randomIndex);
                }
            }

            CaptainList = captainList;
        }

        public void PickCaptain()
        {
            CaptainId = CaptainList[RoundNumber - 1];
        }

        public void PickThemes()
        {
            try
            {
                var jsonContent = File.ReadAllText("./Assets/themes.json");
                var themes = JsonConvert.DeserializeObject<List<Theme>>(jsonContent);

                var themeList = new List<Theme>();

                for (int i = 0; i <= 4; i++)
                {
                    int randomIndex = random.Next(themes.Count);
                    themeList.Add(themes[randomIndex]);
                    themes.RemoveAt(randomIndex);
                }

                ThemeList = themeList;
            }
            catch(Exception e)
            {
                throw;
            }
        }

        public void PickTheme()
        {
            ThemeId = ThemeList[RoundNumber - 1].Id;
        }

        public void DealWeights()
        {
            var weightList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var player in PlayerList)
            {
                int randomIndex = random.Next(weightList.Count);
                player.Weight = weightList[randomIndex];
                weightList.RemoveAt(randomIndex);
            }
        }

        public bool HasEveryoneGuessed()
        {
            return !PlayerList.Any(player => !player.AnswerLocked);
        }

        public void SetRemainingHp()
        {
            var lastWeight = -1;
            foreach (var answer in SortedAnswers)
            {
                var player = PlayerList.First(player => player.ClientId == answer);
                if (lastWeight > player.Weight)
                    RemainingHp--;
                lastWeight = player.Weight;

            }
        }
    }
}
