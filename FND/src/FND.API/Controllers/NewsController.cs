using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FND.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class NewsController : ControllerBase
    {
        private NewsService newsService;

        public NewsController(NewsService newsService)
        {
            this.newsService = newsService;
        }


        [HttpGet]
        public async Task<ActionResult<List<News>>> GetNews([FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var newsList = await newsService.GetNews(IsFakeNewsOnly);
            return Ok(newsList);
        }


        [HttpGet("{publisherId:int}")]
        public async Task<ActionResult<List<ListNewsDto>>> GetNewsByPublisher(int publisherId, [FromQuery(Name = "filter")] string filter)
        {
            var reviewRequestedNewsByPublisher = await newsService.GetNewsByPublisherId(publisherId, filter);
            return Ok(reviewRequestedNewsByPublisher);
        }

        [HttpPost]
        public async Task<ActionResult> PostSendMsg(ClassifyNewsDto classifyNewsDto)//have to give news class object to this 
        {
            var result = await newsService.ClassifyNews(classifyNewsDto);
            return Ok(result);
            //SaveClassifyResult(result);
        }

        [Route("RequestReview")]
        [HttpPost]
        public async Task<IActionResult> RequestReview(CreateRequestReviewDto createRequestReviewDto)
        {
            var request = await newsService.RequestReview(createRequestReviewDto);
            return Ok(request);
        }

        [Route("GetAllReviewRequestedNews")]
        [HttpGet]
        public async Task<ActionResult<List<ListNewsDto>>> GetAllReviewRequestedNews([FromQuery(Name = "filter")] string filter)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var reviewRequestedNewsByPublisher = await newsService.GetAllReviewRequestedNews(filter);
            return Ok(reviewRequestedNewsByPublisher);
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
        [Route("GetNewsCount")]
        [HttpGet]
        public async Task<NewsDashboardResult> GetNewsCountByClassification([FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var newsCountByClassification = await newsService.GetNewsCountByClassification(fromDate, toDate);
            return newsCountByClassification;
        }

        //submit review
        [Route("SubmitReview")]
        [HttpPost]
        public async Task<IActionResult> SubmitReview([FromRoute] int ModeratorId, SubmitReviewDto submitReviewDto)
        {
            var request = await newsService.SubmitReview(ModeratorId, submitReviewDto);
            return Ok(request);
        }

    }
}
