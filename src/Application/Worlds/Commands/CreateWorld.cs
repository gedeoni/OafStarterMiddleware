using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Worlds.DTOs;
using Application.Common.DTOs;
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
            //we can't test id's because they are not created by a user!!
            var world = new World { Name = request.createWorldDto.Name, HasLife = request.createWorldDto.HasLife };
            var createdWorld = await _worldRepository.UpsertDocument(world.Id, world).ConfigureAwait(false);

            _logger.LogInformation($"{new {Entity=createdWorld.Entity, Id=createdWorld.Id, Action="world created", Message="Inserted world in the database"}}");

            EventBusPayload payload = new EventBusPayload{Id = createdWorld.Id, Ref = $"/api/world/{createdWorld.Id}"};
            await _publishEvent.PublishEvent(payload, "Hello.World.Created").ConfigureAwait(false);
            _logger.LogInformation($"{new {Entity=payload.GetType(), Id=payload.Id, Action="world published", Message="World published to the RabbitMq"}}");
            return createdWorld;
        }
    }
}
