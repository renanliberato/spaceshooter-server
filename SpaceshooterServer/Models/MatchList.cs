using System.Collections.Concurrent;
using System.Linq;

namespace SpaceshooterServer.Models
{
    public class MatchList
    {
        public ConcurrentBag<SimplerMatchState> Matches = new ConcurrentBag<SimplerMatchState>();

        public void RemoveConnectionFromMatch(string connectionId)
        {
            foreach (var match in Matches.ToList()) {
                if (match.ConnectionIds.Contains(connectionId)) {
                    System.Console.WriteLine("Removing connection");
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
