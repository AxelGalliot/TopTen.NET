using Stateless;
using TopTen.Server.Models;
using static TopTen.Server.Models.Enums;

namespace TopTen.Server.FiniteStateMachine
{
    public class FiniteStateMachine
    {
        private readonly StateMachine<State, Triggers> machine;

        public Game game;

        public DateTime LastActivityTime { get; set; }

        public FiniteStateMachine(Player player)
        {
            game = new Game(player.ClientId, player);

            LastActivityTime = DateTime.Now;
            
            machine = new StateMachine<State, Triggers>(State.GameReady);

            machine.Configure(State.GameReady)
                .PermitReentry(Triggers.AddPlayer)
                .PermitReentry(Triggers.RemovePlayer)
                .Permit(Triggers.StartGame, State.RoundReady)
                .OnEntryAsync(OnGameReadyEntryAsync)
                .OnExitAsync(OnGameReadyExitAsync);

            machine.Configure(State.RoundReady)
                .Permit(Triggers.StartRound, State.RoundRunning)
                .OnEntryFromAsync(Triggers.StartGame, OnRoundReadyEntryFromGameAsync)
                .OnEntryFromAsync(Triggers.RoundAndHpOk, OnRoundReadyEntryFromRoundAsync)
                .OnExitAsync(OnRoundReadyExitAsync);

            machine.Configure(State.RoundRunning)
                .PermitReentry(Triggers.ReceiveAnswer)
                .Permit(Triggers.EveryoneGuessed, State.RoundEnded)
                .OnEntryAsync(OnRoundRunningEntryAsync)
                .OnExitAsync(OnRoundRunningExitAsync);

            machine.Configure(State.RoundEnded)
                .Permit(Triggers.CaptainGuessed, State.AnswersSorted)
                .OnEntryAsync(OnRoundEndedEntryAsync)
                .OnExitAsync(OnRoundEndedExitAsync);

            machine.Configure(State.AnswersSorted)
                .Permit(Triggers.NoHpLeft, State.Defeat)
                .Permit(Triggers.AllRoundDone, State.Victory)
                .Permit(Triggers.RoundAndHpOk, State.RoundReady)
                .OnEntryAsync(OnAnswersSortedEntryAsync)
                .OnExitAsync(OnAnswersSortedExitAsync);

            machine.Configure(State.Defeat)
                .Permit(Triggers.ReturnToLobby, State.GameReady)
                .OnEntryAsync(OnDefeatEntryAsync)
                .OnExitAsync(OnDefeatExitAsync);

            machine.Configure(State.Victory)
                .Permit(Triggers.ReturnToLobby, State.GameReady)
                .OnEntryAsync(OnVictoryEntryAsync)
                .OnExitAsync(OnVictoryExitAsync);
        }

        private async Task OnGameReadyEntryAsync()
        {

        }
        private async Task OnGameReadyExitAsync()
        {
            
        }
        private async Task OnRoundReadyEntryFromGameAsync()
        {
            game.InitializeGame();
        }
        private async Task OnRoundReadyEntryFromRoundAsync()
        {
            game.InitializeRound();
        }
        private async Task OnRoundReadyExitAsync()
        {

        }
        private async Task OnRoundRunningEntryAsync()
        {
            if (game.HasEveryoneGuessed())
            {
                await machine.FireAsync(Triggers.EveryoneGuessed);
            }
        }
        private async Task OnRoundRunningExitAsync()
        {

        }
        private async Task OnRoundEndedEntryAsync()
        {

        }
        private async Task OnRoundEndedExitAsync()
        {

        }
        private async Task OnAnswersSortedEntryAsync()
        {
            game.SetRemainingHp();
        }
        private async Task OnAnswersSortedExitAsync()
        {

        }
        private async Task OnDefeatEntryAsync()
        {

        }
        private async Task OnDefeatExitAsync()
        {

        }
        private async Task OnVictoryEntryAsync()
        {

        }
        private async Task OnVictoryExitAsync()
        {

        }

        public State GetMachineState()
        {
            return machine.State;
        }

        public async Task StartGameAsync() => await machine.FireAsync(Triggers.StartGame);
        public async Task StartRoundAsync() => await machine.FireAsync(Triggers.StartRound);
        public async Task ReceiveAnswersAsync() => await machine.FireAsync(Triggers.ReceiveAnswer);
        public async Task EveryoneGuessedAsync() => await machine.FireAsync(Triggers.EveryoneGuessed);
        public async Task CaptainGuessedAsync() => await machine.FireAsync(Triggers.CaptainGuessed);
        public async Task AllRoundDoneAsync() => await machine.FireAsync(Triggers.AllRoundDone);
        public async Task NoHpLeftAsync() => await machine.FireAsync(Triggers.NoHpLeft);
        public async Task RoundAndHpOkAsync() => await machine.FireAsync(Triggers.RoundAndHpOk);
        public async Task ReturnToLobbyAsync() => await machine.FireAsync(Triggers.ReturnToLobby);
    }
}
