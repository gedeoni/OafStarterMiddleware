using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Common.Events;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Worlds.EventHandlers
{
    public class ProcessWorldCreatedNotificationHandler : INotificationHandler<WorldReceived>
    {
        private readonly IWorldRepository _worldRepository;
        private readonly ILogger<ProcessWorldCreatedNotificationHandler> _logger;
        private readonly ISendEmails _httpWorldClient;

        public ProcessWorldCreatedNotificationHandler(
            ILogger<ProcessWorldCreatedNotificationHandler> logger,
            IWorldRepository worldRepository,
            ISendEmails httpWorldClient
        )
        {
            _logger = logger;
            _worldRepository = worldRepository;
            _httpWorldClient = httpWorldClient;
        }

        public async Task Handle(WorldReceived notification, CancellationToken cancellation)
        {
            var totalWorlds = await _worldRepository.Count();

            EmailDto emailDto = new EmailDto {
                SenderEmail = "kaninijoe@gmail.com",
                RecipientEmail = "jeremiahchienda@gmail.com",
                Subject = "Testing Email",
                Body = $"the total number of worlds is {totalWorlds}"
            };

            await _httpWorldClient.SendEmail(emailDto);
        }
    }
}