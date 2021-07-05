using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Worlds.Queries
{
    public class GetWorldByIdQuery : IRequest<World>
    {
        public string Id { get; set; }
    }

    public class GetWorldByIdHandler : IRequestHandler<GetWorldByIdQuery, World>
    {
        private readonly ILogger<GetAllWorldsHandler> _logger;
        private readonly IWorldRepository _worldRepo;

        public GetWorldByIdHandler(ILogger<GetAllWorldsHandler> logger, IWorldRepository worldRepo)
        {
            _worldRepo = worldRepo;
            _logger = logger;
        }

        async public Task<World> Handle(GetWorldByIdQuery request, CancellationToken cancellationToken)
        {
            return await _worldRepo.FindOneDocument(request.Id);
        }
    }
}