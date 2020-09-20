using System.Collections.Concurrent;

namespace SpaceshooterServer.Models
{
    public class MatchList
    {
        public ConcurrentBag<SimplerMatchState> Matches = new ConcurrentBag<SimplerMatchState>();
    }
}
