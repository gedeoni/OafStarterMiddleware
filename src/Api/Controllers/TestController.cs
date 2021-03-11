// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// //using Api.Worlds;

// namespace Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class TestController : ControllerBase
//     {
//         public TestController()
//         {
//         }

//         [HttpGet("")]
//         public ActionResult<IEnumerable<TWorld>> GetTWorlds()
//         {
//             return new List<TWorld> { };
//         }

//         [HttpGet("{id}")]
//         public ActionResult<TWorld> GetTWorldById(int id)
//         {
//             return null;
//         }

//         [HttpPost("")]
//         public ActionResult<TWorld> PostTWorld(TWorld World)
//         {
//             return null;
//         }

//         [HttpPut("{id}")]
//         public IActionResult PutTWorld(int id, TWorld World)
//         {
//             return NoContent();
//         }

//         [HttpDelete("{id}")]
//         public ActionResult<TWorld> DeleteTWorldById(int id)
//         {
//             return null;
//         }
//     }
// }