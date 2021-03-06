using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpaceshooterServer.Hubs;
using SpaceshooterServer.Models;

namespace SpaceshooterServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddSingleton<MatchList>();
            services.AddSingleton<SimplerMatchState>();
            services.AddSingleton<ChatList>();
            //services.AddHostedService<TimedSimplerMatchTickService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(settings =>
            {
                settings.WithOrigins(new string[] { "https://renanliberato.com.br", "http://localhost:1234" });
                settings.AllowAnyMethod();
                settings.AllowCredentials();
                settings.AllowAnyHeader();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MatchHub>("/match");
                endpoints.MapHub<SimplerMatchHub>("/simplermatchhub");
                endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }

    public class TimedSimplerMatchTickService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private Timer _timer;
        private readonly IHubContext<SimplerMatchHub> hubContext;
        private readonly SimplerMatchState matchState;

        public TimedSimplerMatchTickService(IHubContext<SimplerMatchHub> hubContext, SimplerMatchState matchState)
        {
            this.hubContext = hubContext;
            this.matchState = matchState;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMilliseconds(100));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
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
