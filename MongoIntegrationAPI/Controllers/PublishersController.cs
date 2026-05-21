using Microsoft.AspNetCore.Mvc;
using MongoIntegrationAPI.Domain.Entities;
using MongoIntegrationAPI.Domain.Interfaces;
using MongoIntegrationAPI.Model;

namespace MongoIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublishersController(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublisher([FromBody] PublisherCreateDto request)
        {
            var publisher = new Publisher { Name = request.Name };
            await _publisherRepository.AddAsync(publisher);

            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, publisher);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisher(string id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher == null) return NotFound();

            return Ok(publisher);
        }

        [HttpGet]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            return Ok(publishers);
        }
    }
}
