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
        private Random random;
        public List<MeteorState> Meteors { get; }
        public List<PlanetState> Planets { get; }
        public List<StarState> Stars { get; }

        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }

        public SimplerMatchState()
        {
            MinX = 0;
            MinY = 0;
            MaxX = 2000;
            MaxY = 2000;
            Players = new List<SimplerPlayerState>();
            random = new Random();
            Planets = new List<PlanetState> {
                new PlanetState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
            };
            Stars = new List<StarState> {
                new StarState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
            };
            Meteors = new List<MeteorState> {
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                },
                new MeteorState {
                    X = random.Next(0, 2000),
                    Y = random.Next(0, 2000),
                }
            };
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

        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "connectionId")]
        public string ConnectionId { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "ship")]
        public string Ship { get; set; }

        [DataMember(Name = "score")]
        public float Score { get; set; }
    }

    [DataContract]
    public class MeteorState
    {
        [DataMember(Name = "x")]
        public float X { get; set; }
        [DataMember(Name = "y")]
        public float Y { get; set; }
    }

    [DataContract]
    public class PlanetState
    {
        [DataMember(Name = "x")]
        public float X { get; set; }
        [DataMember(Name = "y")]
        public float Y { get; set; }
    }

    [DataContract]
    public class StarState
    {
        [DataMember(Name = "x")]
        public float X { get; set; }
        [DataMember(Name = "y")]
        public float Y { get; set; }
    }
}
