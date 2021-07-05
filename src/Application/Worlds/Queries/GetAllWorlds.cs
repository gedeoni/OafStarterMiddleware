using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Worlds.Queries
{
    public class GetAllWorldsQuery : IRequest<IEnumerable<World>>
    {
    }

    public class GetAllWorldsHandler : IRequestHandler<GetAllWorldsQuery, IEnumerable<World>>
    {
        private readonly ILogger<GetAllWorldsHandler> _logger;
        private readonly IWorldRepository _worldRepo;

        public GetAllWorldsHandler(ILogger<GetAllWorldsHandler> logger, IWorldRepository worldRepo)
        {
            _worldRepo = worldRepo;
            _logger = logger;
        }

        public async Task<IEnumerable<World>> Handle(GetAllWorldsQuery request, CancellationToken cancellationToken)
        {
            var worlds = (List<World>)await _worldRepo.FindAllDocuments();

            if (worlds.Count == 0)
            {
                throw new NotFoundException("Worlds Not Found");
            }

            return worlds;
        }
    }
}
