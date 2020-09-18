using Microsoft.AspNetCore.SignalR;
using SpaceshooterServer.Models;
using System.Threading.Tasks;

namespace SpaceshooterServer.Hubs
{
    public class MatchHub : Hub
    {
        private MatchState matchState;

        public MatchHub(MatchState matchState)
        {
            this.matchState = matchState;
        }

        public Task UpdatePlayerCommands(bool acceleratingFrontwards, bool acceleratingBackwards, bool rotatingLeft, bool rotatingRight)
        {
            this.matchState.PlayerState.UpdateCommands(acceleratingFrontwards, acceleratingBackwards, rotatingLeft, rotatingRight);

            return Task.CompletedTask;
        }

        public Task DashLeft()
        {
            this.matchState.PlayerState.DashLeft();

            return Task.CompletedTask;
        }

        public Task DashRight()
        {
            this.matchState.PlayerState.DashRight();

            return Task.CompletedTask;
        }
    }
}
