using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Application.Common.DTOs;
using Application.Common.Events;
using MediatR;
using Newtonsoft.Json;
using sdk.MessageHandler;
using Serilog;
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
                string action = routingKey.Split('.')[2];
                var payload = JsonConvert.DeserializeObject<EventBusPayload>(message);

                _logger.LogInformation($"{new {Entity=payload.GetType(),Id=payload.Id,Action="Receive message",Message="RabbitMQ message received"}}");
                var worldCreatedNotification = new WorldCreatedNotification(payload, action);

                await _mediator.Publish(worldCreatedNotification).ConfigureAwait(false);
                _logger.LogInformation($"{new {Entity=worldCreatedNotification.GetType(),Action="Publish EventBusnotification", Message="worldCreatedNotification published"}}");
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
