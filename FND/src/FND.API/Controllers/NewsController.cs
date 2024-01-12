using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FND.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class NewsController : ControllerBase
    {
        private INewsService newsService;

        public NewsController(INewsService newsService)
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

        [Authorize]
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

        [Authorize]
        [Route("RequestReview")]
        [HttpPost]
        public async Task<IActionResult> RequestReview(CreateRequestReviewDto createRequestReviewDto)
        {
            var request = await newsService.RequestReview(createRequestReviewDto);
            return Ok(request);
        }

        [Authorize]
        [Route("GetAllReviewRequestedNews")]
        [HttpGet]
        public async Task<ActionResult<List<ListNewsDto>>> GetAllReviewRequestedNews([FromQuery(Name = "filter")] string filter)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            var reviewRequestedNewsByPublisher = await newsService.GetAllReviewRequestedNews(filter);
            return Ok(reviewRequestedNewsByPublisher);
        }


        [Authorize]
        [Route("GetNewsCount")]
        [HttpGet]
        public async Task<ActionResult<NewsDashboardResultDto>> GetNewsCountByClassification(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            if (userId == null)
                return BadRequest();

            var newsCountByClassification = await newsService.GetNewsCountByClassification(userId, fromDate, toDate);
            return newsCountByClassification;
        }

        [Authorize]
        [Route("GetReviewRequestCount")]
        [HttpGet]
        public async Task<ActionResult<ReviewRequestDashboardRestultDto>> GetReviewRequestCount(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            if (userId == null)
                return BadRequest();

            var reviewREquestCountByStatus = await newsService.GetReviewRequestCount(userId, fromDate, toDate);
            return reviewREquestCountByStatus;
        }
        [Authorize]
        [Route("GetNewsClassificationCountByPublisher")]
        [HttpGet]
        public async Task<ActionResult<List<NewsCountByPublisherDashboardresultDto>>> GetNewsClassificationCountByPublisher(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            if (userId == null)
                return BadRequest();

            var newsClassificationCountByPublisher = await newsService.GetNewsClassificationCountByPublisher(userId, fromDate, toDate);
            return newsClassificationCountByPublisher;
        }

        [Route("GetReviewRequestCountByPublisher")]
        [HttpGet]
        public async Task<ActionResult<List<ReviewRequestCountByPublisherDashboardresultDto>>> GetReviewRequestCountByPublisher(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            //var newsList = await _fNDDBContext.News.ToListAsync();
            if (userId == null)
                return BadRequest();

            var reviewRequestCountByPublisher = await newsService.GetReviewRequestCountByPublisher(userId, fromDate, toDate);
            return reviewRequestCountByPublisher;
        }

        [Authorize]
        [Route("GetNewsClassificationCountByMonth")]
        [HttpGet]
        public async Task<ActionResult<List<NewsCountByMonthDashboardresultDto>>> GetNewsClassificationCountByMonth(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            if (userId == null)
                return BadRequest();

            var newsClassificationCountByMonth = await newsService.GetNewsClassificationCountByMonth(userId, fromDate, toDate);
            return newsClassificationCountByMonth;
        }

        //submit review
        [Authorize]
        [Route("SubmitReview")]
        [HttpPost]
        public async Task<IActionResult> SubmitReview(SubmitReviewDto submitReviewDto)
        {
            var request = await newsService.SubmitReview(submitReviewDto);
            return Ok(request);
        }

    }
}
