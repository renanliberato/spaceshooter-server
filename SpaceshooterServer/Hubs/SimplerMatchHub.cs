using Microsoft.AspNetCore.SignalR;
using SpaceshooterServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceshooterServer.Hubs
{
    public class SimplerMatchHub : Hub
    {
        private MatchList matchList;

        public SimplerMatchHub(MatchList matchList)
        {
            this.matchList = matchList;
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            this.matchList.RemoveConnectionFromMatch(Context.ConnectionId, this);
        }

        public Task AddShipToGame(string matchId, Guid shipId, string username, string ship)
        {
            var match = this.matchList.Matches.First(m => m.MatchId == matchId);

            match.ConnectionIds.Add(Context.ConnectionId);

            match.Players.Add(new SimplerPlayerState(match)
            {
                ConnectionId = Context.ConnectionId,
                Id = shipId,
                Username = username,
                Ship = ship,
                MaxHealth = 10
            });

            // notify other players that a new ship entered.
            GetOtherMatchConnections(match).SendAsync("ShipAddedtoGame", shipId, username, ship);

            // tell the player which other ships are on the game right now.
            match.Players.ToArray().Where(p => p.Id != shipId).ToList().ForEach(player =>
            {
                Clients.Client(Context.ConnectionId).SendAsync("ShipAddedtoGame", player.Id, player.Username, player.Ship);
            });

            return Task.CompletedTask;
        }

        public Task SendEventToOtherPlayers(string matchId, object @event)
        {
            var match = this.matchList.Matches.First(m => m.MatchId == matchId);

            GetOtherMatchConnections(match).SendAsync("EventBroadcasted", @event);

            return Task.CompletedTask;
        }

        public Task UpdateShipPosition(string matchId, Guid shipId, object properties)
        {
            var match = this.matchList.Matches.First(m => m.MatchId == matchId);

            GetOtherMatchConnections(match).SendAsync("ShipPositionUpdated", shipId, properties);

            return Task.CompletedTask;
        }

        public Task FireShot(string matchId, Guid shipId, float x, float y, float angle)
        {
            var match = this.matchList.Matches.First(m => m.MatchId == matchId);

            GetOtherMatchConnections(match).SendAsync("ShotFired", shipId, x, y, angle);

            return Task.CompletedTask;
        }

        public IClientProxy GetOtherMatchConnections(SimplerMatchState match)
        {
            return Clients.Clients(match.ConnectionIds.Except(new string[] { Context.ConnectionId }).ToArray());
        }

        public Task DestroyPlayer(string matchId, Guid shipId)
        {
            var match = this.matchList.Matches.First(m => m.MatchId == matchId);
            match.Players.Remove(match.Players.First(p => p.Id == shipId));
            match.ConnectionIds = new System.Collections.Concurrent.ConcurrentBag<string>(match.ConnectionIds.Except(new[] { Context.ConnectionId }));

            GetOtherMatchConnections(match).SendAsync("PlayerDestroyed", shipId, match.Players.Count);

            return Task.CompletedTask;
        }
    }
}
