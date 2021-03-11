using Application.Common.Events;
using System.Threading.Tasks;
using System.Threading;
using System.Text.Json;
using MediatR;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Application.Common.DTOs;

namespace Application.Worlds.Commands
{
    public class ProcessWorldCreatedNotificationHandler : INotificationHandler<WorldCreatedNotification>
    {
        private readonly IWorldRepository _worldRepository;
        private readonly ILogger<ProcessWorldCreatedNotificationHandler> _logger;
        private readonly IHttpWorldClient _httpWorldClient;

        public ProcessWorldCreatedNotificationHandler(
            ILogger<ProcessWorldCreatedNotificationHandler> logger,
            IWorldRepository worldRepository,
            IHttpWorldClient httpWorldClient
        )
        {
            _logger = logger;
            _worldRepository = worldRepository;
            _httpWorldClient = httpWorldClient;

        }

        public async Task Handle(WorldCreatedNotification notification, CancellationToken cancellation)
        {
            var totalWorlds = await _worldRepository.GetTotalWorlds("World");
            _logger.LogInformation($"Payload received: {notification.eventBusPayload} action: {notification.action}");

            await foreach (var number in totalWorlds)
            {
                _logger.LogInformation($"The number of total worlds is: {number}");
                await _httpWorldClient.SendEmail(new EmailDto
                    {
                        SenderEmail="kaninijoe@gmail.com",
                        RecipientEmail="gedeoniyonkuru@gmail.com",
                        Subject="Testing Email",
                        Body=$"the total number of worlds is {number}"
                    });
            }
        }
    }
}