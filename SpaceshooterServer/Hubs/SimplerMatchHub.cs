using Microsoft.AspNetCore.SignalR;
using SpaceshooterServer.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceshooterServer.Hubs
{
    public class SimplerMatchHub : Hub
    {
        private SimplerMatchState matchState;

        public SimplerMatchHub(SimplerMatchState matchState)
        {
            this.matchState = matchState;
        }

        public Task AddShipToGame(Guid shipId)
        {
            this.matchState.Players.Add(new SimplerPlayerState(this.matchState)
            {
                Id = shipId,
                MaxHealth = 10
            });

            Clients.AllExcept(new string[] { Context.ConnectionId }).SendAsync("ShipAddedtoGame", shipId);

            this.matchState.Players.ToArray().Where(p => p.Id != shipId).ToList().ForEach(player =>
            {
                Clients.Client(Context.ConnectionId).SendAsync("ShipAddedtoGame", player.Id);
            });

            return Task.CompletedTask;
        }

        public Task UpdateShipPosition(Guid shipId, float x, float y, float angle, float health)
        {
            this.matchState.Players.FirstOrDefault(p => p.Id == shipId)?.UpdatePosition(x, y, angle, health);

            Clients.AllExcept(new string[] { Context.ConnectionId }).SendAsync("ShipPositionUpdated", shipId, x, y, angle, health);

            return Task.CompletedTask;
        }

        public Task FireShot(Guid shipId, float x, float y, float angle)
        {
            Clients.AllExcept(new string[] { Context.ConnectionId }).SendAsync("ShotFired", shipId, x, y, angle);

            return Task.CompletedTask;
        }
    }
}
