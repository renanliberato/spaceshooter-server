using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SpaceshooterServer.Models
{
    public class SimplerMatchState
    {
        public string MatchId { get; set; }
        public ConcurrentBag<string> ConnectionIds { get; set; } = new ConcurrentBag<string>();
        public List<SimplerPlayerState> Players { get; set; }

        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public SimplerMatchState()
        {
            MinX = 0;
            MinY = 0;
            MaxX = 1000;
            MaxY = 1000;
            Players = new List<SimplerPlayerState>();
        }
    }

    public class SimplerPlayerState
    {
        private SimplerMatchState matchState;

        public SimplerPlayerState(SimplerMatchState matchState)
        {
            this.matchState = matchState;
        }

        internal void UpdatePosition(float x, float y, float dx, float dy, float angle, float health)
        {
            this.X = x;
            this.Y = y;
            this.Dx = dx;
            this.Dy = dy;
            this.Angle = angle;
            this.Health = health;
        }

        public Guid Id { get; set; }
        public float Angle { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Dx { get; set; }
        public float Dy { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
    }
}
