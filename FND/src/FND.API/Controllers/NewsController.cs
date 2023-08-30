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
        public async Task<ActionResult<List<News>>> GetNewsByPublisher(int publisherId, [FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var newsList = await newsService.GetNewsByPublisher(publisherId, IsFakeNewsOnly);
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

        [Route("RequestReview")]
        [HttpPost]
        public async Task<IActionResult> RequestReview(CreateRequestReviewDto createRequestReviewDto)
        {
            var request = await newsService.RequestReview(createRequestReviewDto);
            return Ok(request);
        }

        [Route("GetReviewRequestedNewsByPublisherId")]
        [HttpGet]
        public async Task<ActionResult<List<ReviewRequest>>> GetReviewRequestedNewsByPublisherId(int userId)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var reviewRequestedNewsByPublisher = await newsService.GetReviewRequestedNewsByPublisherId(userId);
            return Ok(reviewRequestedNewsByPublisher);
        }

        [Route("GetAllReviewRequestedNews")]
        [HttpGet]
        public async Task<ActionResult<List<ReviewRequest>>> GetAllReviewRequestedNews()
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var reviewRequestedNewsByPublisher = await newsService.GetAllReviewRequestedNews();
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
        public async Task<List<NewsClassificationCount>> GetNewsCountByClassification([FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var newsCountByClassification = await newsService.GetNewsCountByClassification(fromDate, toDate);
            return newsCountByClassification;
        }

        //submit review
        [Route("SubmitReview")]
        [HttpPost]
        public async Task<IActionResult> SubmitReview(SubmitReviewDto submitReviewDto)
        {
            var request = await newsService.SubmitReview(submitReviewDto);
            return Ok(request);
        }

    }
}
