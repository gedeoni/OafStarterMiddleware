using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Worlds.Commands;
using Application.Worlds.DTOs;
using Application.Worlds.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorldController : ControllerBase
    {
        private readonly ILogger<WorldController> _logger;
        private readonly IMediator _mediator;

        public WorldController(ILogger<WorldController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        async public Task<List<World>> Index()
        {
            _logger.LogInformation("Getting a list of worlds");
            return await _mediator.Send(new GetAllWorlds()).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<World> Create(CreateWorldDto createWorldDto)
        {
            return await _mediator.Send(new CreateWorldComand(createWorldDto)).ConfigureAwait(false);
        }
    }
}
