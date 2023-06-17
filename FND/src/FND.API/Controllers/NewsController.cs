using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FND.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class NewsController : ControllerBase
    {
        private readonly FNDDBContext _fNDDBContext;
        private NewsService newsService;

        public NewsController(FNDDBContext fNDDBContext)
        {
            newsService = new NewsService(fNDDBContext);
        }


        [HttpGet]
        public async Task<ActionResult<List<News>>> GetNews()
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var newsList = await newsService.GetNews();
            return Ok(newsList);
        }
        //var news1 = new  { topic = "covid 19 is every wheeersjgnds", publisher = "Rajitha" };
        //var news2 = new { topic = "Everyone should put vaccine", publisher = "Senthalan" };

        [HttpPost]
        public async Task<ActionResult> PostSendMsg(ClassifyNewsDto classifyNewsDto)//have to give news class object to this 
        {
            var result = await newsService.ClassifyNews(classifyNewsDto);
            return Ok(result);
            //SaveClassifyResult(result);
        }

        //[Route("AddNews")]
        //[HttpPost]
        //public async Task<IActionResult> CreateNews(News newsRequest)//give values except id. delete id:
        //{

        //    await _fNDDBContext.News.AddAsync(newsRequest);
        //    await _fNDDBContext.SaveChangesAsync();

        //    return Ok(newsRequest);

        //}

        //[Route("Update")]
        //[HttpPatch("{id}")]
        //public async Task<IActionResult> PatchEmployee([FromRoute] int id, [FromBody] JsonPatchDocument employeeDocument)
        //{
        //    var updatedEmployee = await _newsRepository.UpdateEmployeePatchAsync(id, employeeDocument);
        //    if (updatedEmployee == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(updatedEmployee);
        //}

    }
}
