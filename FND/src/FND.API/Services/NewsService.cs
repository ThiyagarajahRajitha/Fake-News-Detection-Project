using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace FND.API.Services
{
    public interface NewsService
    {
        public Task<string> ClassifyNews(ClassifyNewsDto classifyNewsDto);

        public Task<List<News>> GetNews([FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly);

        public Task<List<NewsClassificationCount>> GetNewsCountByClassification([FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate);

        public Task<List<News>> GetNewsByPublisher(int publisherId, [FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly);

        public Task<ReviewRequest> RequestReview(CreateRequestReviewDto createRequestReviewDto);

        public Task<ReviewRequest> SubmitReview(SubmitReviewDto submitReviewDto);

        public Task<List<ReviewRequest>> GetReviewRequestedNewsByPublisherId(int userId);
        public Task<List<ReviewRequest>> GetAllReviewRequestedNews();
    }
}