using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpaceshooterServer.Models
{
    public class ChatList
    {
        public List<ChatMessage> Messages { get; } = new List<ChatMessage>();
        public ConcurrentBag<string> ConnectionIds { get; set; } = new ConcurrentBag<string>();
    }

    [DataContract]
    public class ChatMessage
    {
        [DataMember(Name = "id")]
        public Guid Id { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
