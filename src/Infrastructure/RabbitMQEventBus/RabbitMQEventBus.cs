using sdk.MessagePublisher;
using Application.Common.Interfaces;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.RabbitMqEventBus
{
    public class RabbitMQEventBus : IPublishEvent
    {
        private readonly IRabbitMQMessagePublisher _messagePublisher;
        private readonly IConfiguration _config;
        public RabbitMQEventBus(IRabbitMQMessagePublisher messagePublisher,
        IConfiguration config
        )
        {
            _messagePublisher = messagePublisher;
            _config = config;
        }

        public async Task PublishEvent(object payload, string routingKey)
        {
            await _messagePublisher.PublishMessageAsync(payload,routingKey);
        }
    }
}