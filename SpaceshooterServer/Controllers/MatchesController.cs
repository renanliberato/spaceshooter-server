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
    public class MatchesController : ControllerBase
    {
        private readonly MatchList matchList;

        public MatchesController(MatchList matchList)
        {
            this.matchList = matchList;
        }

        [HttpPost]
        [Route("create")]
        public Task CreateMatch([FromBody] string matchId)
        {
            if (!this.matchList.Matches.Any(m => m.MatchId == matchId))
                this.matchList.Matches.Add(new SimplerMatchState { MatchId = matchId });

            return Task.CompletedTask;
        }
    }
}
