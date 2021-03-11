using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Worlds.Queries
{
    public class GetAllWorlds : IRequest<List<World>>
    {
    }

    public class GetAllWorldsHandler : IRequestHandler<GetAllWorlds, List<World>>
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetAllWorldsHandler> _logger;

        public GetAllWorldsHandler(IConfiguration configuration, ILogger<GetAllWorldsHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public Task<List<World>> Handle(GetAllWorlds request, CancellationToken cancellationToken)
        {
            var name = _configuration["MyWorld:Name"] ?? "Earth Default";
            var hasLife = bool.Parse(_configuration["MyWorld:HasLife"]);

            return Task.FromResult(new List<World>() { new World {
                Name = name,
                HasLife = hasLife
            } });
        }
    }
}
