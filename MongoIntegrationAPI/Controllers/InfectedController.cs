using Microsoft.AspNetCore.Mvc;
using MongoIntegrationAPI.Domain;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectedController : ControllerBase
    {
        private readonly IInfectedRepository _infectedRepository;

        public InfectedController(IInfectedRepository infectedRepository)
        {
            _infectedRepository = infectedRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SaveInfectedAsync([FromBody] InfectedDto request)
        {
            var infected = new Infected(request.Birthday, request.Sex, request.Latitude, request.Longitude);
            await _infectedRepository.Add(infected);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetInfectedsAsync()
        {
            var infecteds = await _infectedRepository.GetAll();
            return Ok(infecteds);
        }
    }
}
