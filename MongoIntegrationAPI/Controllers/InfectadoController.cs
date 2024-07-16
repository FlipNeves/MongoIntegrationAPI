using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoIntegrationAPI.Data.Collections;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InfectadoController : ControllerBase
    {
        private readonly MongoDb _mongoDb;
        private readonly IMongoCollection<Infectado> _infectadosCollection;

        public InfectadoController(MongoDb mongoDb)
        {
            _mongoDb = mongoDb;
            _infectadosCollection = _mongoDb.Db.GetCollection<Infectado>(typeof(Infectado).Name.ToLower());
        }

        [HttpPost]
        public async Task<IActionResult> SaveInfectadoAsync([FromBody] InfectadoDto request)
        {
            var infectado = new Infectado(request.Birthday, request.Sex, request.Latitude, request.Longitude);
            await _infectadosCollection.InsertOneAsync(infectado);

            return Created();
        }

        [HttpGet]
        public async Task<ActionResult> GetInfectadosAsync()
        {
            var infectados = await _infectadosCollection.Find(Builders<Infectado>.Filter.Empty).ToListAsync();
            return Ok(infectados);
        }
    }
}
