using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using FND.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FND.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private PublisherService publisherService;
        public PublisherController(FNDDBContext fNDDBContext) 
        {
            publisherService = new PublisherService(fNDDBContext);
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetNews([FromQuery(Name = "PendingApproval")] bool IsPendingOnly)
        {
            var publishersList = await publisherService.GetPublishers(IsPendingOnly);
            return publishersList;
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> UpdatePublisher([FromRoute] int id)
        {
            await publisherService.UpdatePublisherAsync(id);
            return Ok();
        }
    }
}
