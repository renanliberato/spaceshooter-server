using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpaceshooterServer.Models;

namespace SpaceshooterServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatList chatList;

        public ChatController(ChatList chatList)
        {
            this.chatList = chatList;
        }

        [HttpGet]
        public IEnumerable<ChatMessage> GetMatches()
        {
            return this.chatList.Messages;
        }
    }
}
