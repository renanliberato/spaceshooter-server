using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SpaceshooterServer.Models
{
    public class SimplerMatchState
    {
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

        internal void UpdatePosition(float x, float y, float angle, float health)
        {
            this.X = x;
            this.Y = y;
            this.Angle = angle;
            this.Health = health;
        }

        public Guid Id { get; set; }
        public float Angle { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
    }
}
