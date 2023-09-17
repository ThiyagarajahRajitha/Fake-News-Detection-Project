using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using FND.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Encodings;

namespace FND.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private IPublisherService publisherService;
        public PublisherController(IPublisherService publisherService)
        {
            this.publisherService = publisherService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetPublisher([FromQuery(Name = "PendingApproval")] bool IsPendingOnly)
        {
            var publishersList = await publisherService.GetPublishers(IsPendingOnly);
            return publishersList;
        }

        [Route("GetPublication")]
        [HttpGet]
        public async Task<ActionResult<List<Publication>>> GetPublication()
        {
            var publishersList = await publisherService.GetPublication();
            return publishersList;
        }

        [Authorize]
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> UpdatePublisher([FromRoute] int id, [FromBody] ActivatePublisherDto activatePublisher)
        {
            if (activatePublisher == null)
                return BadRequest();
            await publisherService.UpdatePublisherAsync(id, activatePublisher);
            return Ok();
        }

        [Authorize]
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

        [Authorize]
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
