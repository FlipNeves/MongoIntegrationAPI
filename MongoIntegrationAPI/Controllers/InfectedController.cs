using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoIntegrationAPI.Data.Collections;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectedController : ControllerBase
    {
        private readonly MongoDb _mongoDb;
        private readonly IMongoCollection<Infected> _infectedsCollection;

        public InfectedController(MongoDb mongoDb)
        {
            _mongoDb = mongoDb;
            _infectedsCollection = _mongoDb.Db.GetCollection<Infected>(typeof(Infected).Name.ToLower());
        }

        [HttpPost]
        public async Task<IActionResult> SaveInfectedAsync([FromBody] InfectedDto request)
        {
            var infected = new Infected(request.Birthday, request.Sex, request.Latitude, request.Longitude);
            await _infectedsCollection.InsertOneAsync(infected);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetInfectedsAsync()
        {
            var infecteds = await _infectedsCollection.Find(Builders<Infected>.Filter.Empty).ToListAsync();
            return Ok(infecteds);
        }
    }
}
