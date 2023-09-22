using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FND.API.Interfaces
{
    public interface INewsService
    {
        public Task<string> ClassifyNews(ClassifyNewsDto classifyNewsDto);

        public Task<List<News>> GetNews([FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly);

        public Task<NewsDashboardResultDto> GetNewsCountByClassification(int id, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate);
        public Task<ReviewRequestDashboardRestultDto> GetReviewRequestCount(int id, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate);
        public Task<List<NewsCountByPublisherDashboardresultDto>> GetNewsClassificationCountByPublisher(int id, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate);
        public Task<List<NewsCountByMonthDashboardresultDto>> GetNewsClassificationCountByMonth(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate);
        public Task<List<ListNewsDto>> GetNewsByPublisherId(int publisherId, string Filter);
        public Task<ReviewRequest> RequestReview(CreateRequestReviewDto createRequestReviewDto);

        public Task<ReviewRequest> SubmitReview(SubmitReviewDto submitReviewDto);

        public Task<List<ListNewsDto>> GetAllReviewRequestedNews(string filter);
        public Task<List<ReviewRequestCountByPublisherDashboardresultDto>> GetReviewRequestCountByPublisher(int userId, string fromDate, string toDate);
    }
}