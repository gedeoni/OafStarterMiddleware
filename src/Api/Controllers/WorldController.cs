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
    public class WorldsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorldsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        async public Task<ActionResult<IEnumerable<World>>> GetWorlds()
        {
            return await _mediator.Send(new GetAllWorlds());
        }

        [HttpPost]
        public async Task<ActionResult<World>> PostWorld(CreateWorldDto createWorldDto)
        {
            var world = await _mediator.Send(new CreateWorldComand(createWorldDto));
            return new CreatedResult("api/worlds", world);
        }
    }
}
