using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Common.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sdk.MessageHandler;
namespace WorldConsumer
{
    public class WorldConsumer : IHostedService, IMessageHandlerCallback
    {
        private readonly ILogger<WorldConsumer> _logger;
        private readonly IRabbitMqMessageHandler _rabbitMqMessageHandler;
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public WorldConsumer(
            ILogger<WorldConsumer> logger,
            IMediator mediator,
            IRabbitMqMessageHandler rabbitMqMessageHandler,
            IConfiguration config
            )
        {
            _logger = logger;
            _rabbitMqMessageHandler = rabbitMqMessageHandler;
            _mediator = mediator;
            _config = config;
        }

        async public Task HandleMessageAsync(string routingKey, string message)
        {
            try
            {
                var payload = JsonConvert.DeserializeObject<EventBusPayload>(message);
                await _mediator
                    .Publish(new WorldReceived(payload, routingKey.Split('.')[2]))
                    .ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitMqMessageHandler.Start(this);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqMessageHandler.Stop();
            return Task.CompletedTask;
        }
    }
}
