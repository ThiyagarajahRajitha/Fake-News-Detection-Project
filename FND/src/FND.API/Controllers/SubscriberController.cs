using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FND.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        private SubscribeService subscribeService;
        public SubscriberController(FNDDBContext fNDDBContext) { 

            subscribeService = new SubscribeService(fNDDBContext);
        }

        [HttpPost]
        public async Task<ActionResult> Subscribe(CreateSubscriberDto createSubscriberDto)//have to give news class object to this 
        {
            var result = await subscribeService.Subscribe(createSubscriberDto);
            return Ok(result);
        }
    }
}
