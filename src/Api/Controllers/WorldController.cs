using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Worlds.Commands;
using Application.Worlds.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            var worlds = await _mediator.Send(new GetAllWorldsQuery());
            return Ok(worlds);
        }

        [HttpGet("{id}")]
        async public Task<ActionResult<World>> GetWorld(string id)
        {
            var world = await _mediator.Send(new GetWorldByIdQuery() { Id = id });
            return Ok(world);
        }

        [HttpPost]
        public async Task<ActionResult<World>> PostWorld(CreateWorldDto createWorldDto)
        {
            var world = await _mediator.Send(new CreateWorldComand(createWorldDto));
            return new CreatedResult("api/worlds", world);
        }
    }
}
