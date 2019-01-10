using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreConsoleEngine.Services
{
    public abstract class BackgroudLoopService : IHostedService, IDisposable
    {
        private readonly CancellationTokenSource _stoppingCts;

        private Task _executingTask;

        public IServiceProvider Services { get; }

        public BackgroudLoopService(IServiceProvider services)
        {
            Services = services;
            _stoppingCts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                _stoppingCts.Cancel();
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteLoopTask(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }
            return Task.CompletedTask;
        }

        private async Task ExecuteLoopTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Run(ExecuteAsync, token);
            }
        }

        protected abstract Task ExecuteAsync();

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
            {
                return;
            }

            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, 
                    Task.Delay(Timeout.Infinite,cancellationToken));
            }
        }

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }
}
