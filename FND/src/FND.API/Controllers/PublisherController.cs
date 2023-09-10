using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using FND.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;

namespace FND.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private PublisherService publisherService;
        public PublisherController(PublisherService publisherService)
        {
            this.publisherService = publisherService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetPublisher([FromQuery(Name = "PendingApproval")] bool IsPendingOnly)
        {
            var publishersList = await publisherService.GetPublishers(IsPendingOnly);
            return publishersList;
        }

        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> UpdatePublisher([FromRoute] int id, [FromBody] ActivatePublisherDto activatePublisher)
        {
            if (activatePublisher == null)
                return BadRequest();
            await publisherService.UpdatePublisherAsync(id, activatePublisher);
            return Ok();
        }


        [HttpPatch("{id:int}/reject")]
        public async Task<IActionResult> RejectPublisher(int id, RejectPublisherDto rejectPublisherDto)
        {
            if(rejectPublisherDto==null)
                return BadRequest();

            bool rslt = await publisherService.RejectPublisher(id, rejectPublisherDto);
            if(rslt == true)
                return Ok();
            else
                return BadRequest(rslt);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePublisher([FromRoute] int id)
        {
            bool rslt =  await publisherService.DeletePublisher(id);
            if(rslt == true)
                return Ok();
            else
                return BadRequest(rslt);
        }
    }
}
