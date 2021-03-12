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
            //_logger.LogInformation($"{new {Entity=typeof(notification.eventBusPayload),Id=notification.eventBusPayload.Id,Action="Event Bus Payload received"}}");

            await foreach (var number in totalWorlds)
            {
                _logger.LogInformation($" {new {Action="Total worlds fetched", Message=$"the total worlds number is {number}"}}");
                EmailDto emailDto = new EmailDto
                    {
                        SenderEmail="kaninijoe@gmail.com",
                        RecipientEmail="gedeoniyonkuru@gmail.com",
                        Subject="Testing Email",
                        Body=$"the total number of worlds is {number}"
                    };
                await _httpWorldClient.SendEmail(emailDto);
                _logger.LogInformation($" {new {Action="Worlds number email sent", Message=$"the total worlds number email sent to {emailDto.SenderEmail}"}}");
            }
        }
    }
}