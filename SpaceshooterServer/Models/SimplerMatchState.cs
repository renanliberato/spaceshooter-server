using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

    [DataContract]
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

        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "connectionId")]
        public string ConnectionId { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "angle")]
        public float Angle { get; set; }

        [DataMember(Name = "x")]
        public float X { get; set; }

        [DataMember(Name = "y")]
        public float Y { get; set; }

        [DataMember(Name = "dx")]
        public float Dx { get; set; }

        [DataMember(Name = "dy")]
        public float Dy { get; set; }

        [DataMember(Name = "health")]
        public float Health { get; set; }

        [DataMember(Name = "maxHealth")]
        public float MaxHealth { get; set; }
    }
}
