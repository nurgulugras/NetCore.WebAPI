using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ALMS.Service
{
    public class SessionUpdaterWorkerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Background Service for Task
        /// </summary>
        public SessionUpdaterWorkerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var timeoutMillisecondsDelay = 90;
                await Task.Delay(timeoutMillisecondsDelay, stoppingToken);

                await DoWorkAsync();
            }
        }

        private async Task DoWorkAsync()
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var sessionUpdateQueueService = scope.ServiceProvider.GetService<ISessionUpdateQueueService>();
                if (sessionUpdateQueueService.HasAny())
                {
                    using (var sessionService = scope.ServiceProvider.GetService<ISessionService>())
                    {
                        var sessionCount = sessionUpdateQueueService.Count;
                        for (int i = 0; i < sessionCount; i++)
                        {
                            var nextSession = sessionUpdateQueueService.GetBeginSessionAndRemove();
                            await sessionService.UpdateSessionAsync(nextSession);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                var exTsr = ex.ToString();
            }
        }
    }
}