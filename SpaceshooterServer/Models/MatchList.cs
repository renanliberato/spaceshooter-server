using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Linq;
using SpaceshooterServer.Hubs;

namespace SpaceshooterServer.Models
{
    public class MatchList
    {
        public ConcurrentBag<SimplerMatchState> Matches = new ConcurrentBag<SimplerMatchState>();

        public void RemoveConnectionFromMatch(string connectionId, SimplerMatchHub hub)
        {
            foreach (var match in Matches.ToList()) {
                if (match.ConnectionIds.Contains(connectionId)) {
                    var thePlayer = match.Players.FirstOrDefault(p => p.ConnectionId == connectionId);
                    
                    if (thePlayer != null) {
                        System.Console.WriteLine($"Client Disconnected: {hub.Context.ConnectionId}");
                        hub.GetOtherMatchConnections(match).SendAsync("ShipQuitTheGame", thePlayer.Id);
                        match.Players.Remove(thePlayer);
                    }

                    match.ConnectionIds = new ConcurrentBag<string>(match.ConnectionIds.Except(new[]{connectionId}));
                }

                if (!match.ConnectionIds.Any()) {
                    System.Console.WriteLine("Will remove match");
                    Matches = new ConcurrentBag<SimplerMatchState>(Matches.Except(new[]{match}));
                }
            }
        }
    }
}
