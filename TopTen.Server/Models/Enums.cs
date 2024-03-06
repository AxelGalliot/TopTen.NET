using System.ComponentModel.DataAnnotations;

namespace TopTen.Server.Models
{
    public static class Enums
    {
        public enum State
        {
            [Display(Name="GameReady")]
            GameReady,
            [Display(Name = "RoundReady")]
            RoundReady,
            [Display(Name = "RoundRunning")]
            RoundRunning,
            [Display(Name = "RoundEnded")]
            RoundEnded,
            [Display(Name = "AnswersSorted")]
            AnswersSorted,
            [Display(Name = "Victory")]
            Victory,
            [Display(Name = "Defeat")]
            Defeat,
        }

        public enum Triggers
        {
            AddPlayer,
            RemovePlayer,
            StartGame,
            StartRound,
            ReceiveAnswer,
            EveryoneGuessed,
            CaptainGuessed,
            RoundAndHpOk,
            AllRoundDone,
            NoHpLeft,
            ReturnToLobby
        }
    }
}
