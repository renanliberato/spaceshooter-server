using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SpaceshooterServer.Hubs;
using SpaceshooterServer.Models;

namespace SpaceshooterServer
{
    public class TimedMatchTickService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer;
        private readonly IHubContext<MatchHub> hubContext;
        private readonly MatchState matchState;

        public TimedMatchTickService(IHubContext<MatchHub> hubContext, MatchState matchState)
        {
            this.hubContext = hubContext;
            this.matchState = matchState;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMilliseconds(16));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            this.matchState.Tick();

            Task.Run(async () =>
            {
                await this.hubContext.Clients.All.SendAsync("MatchStateUpdated", JsonSerializer.Serialize(this.matchState));
            });
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
