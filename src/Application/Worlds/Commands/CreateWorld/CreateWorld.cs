using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Worlds.Commands;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Worlds.Commands
{
    public class CreateWorldComand : IRequest<World>
    {
        public readonly CreateWorldDto createWorldDto;

        public CreateWorldComand(CreateWorldDto createWorldDto)
        {
            this.createWorldDto = createWorldDto;
        }
    }

    public class CreateWorldComandHandler : IRequestHandler<CreateWorldComand, World>
    {
        private const string RoutingKey = "hello.world.created";
        private readonly ILogger<CreateWorldComandHandler> _logger;
        private readonly IWorldRepository _worldRepository;
        private readonly IPublishEvent _publishEvent;

        public CreateWorldComandHandler(ILogger<CreateWorldComandHandler> logger, IWorldRepository worldRepository, IPublishEvent publishEvent)
        {
            _logger = logger;
            _worldRepository = worldRepository;
            _publishEvent = publishEvent;
        }

        async public Task<World> Handle(CreateWorldComand request, CancellationToken cancellationToken)
        {
            World world = MapWorldFromDto(request);

            var createdWorld = await _worldRepository.InsertDocument(world);

            EventBusPayload payload = GetPayload(createdWorld.Id);

            await _publishEvent.PublishEvent(payload, RoutingKey);

            return createdWorld;
        }

        private static World MapWorldFromDto(CreateWorldComand request)
        {
            return new World {
                Name = request.createWorldDto.Name,
                HasLife = request.createWorldDto.HasLife
            };
        }

        private static EventBusPayload GetPayload(string id) => new EventBusPayload {
            Id = id,
            Ref = $"/api/world/{id}"
        };
    }
}
