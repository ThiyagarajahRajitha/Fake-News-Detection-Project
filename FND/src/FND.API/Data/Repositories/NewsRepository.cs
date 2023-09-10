﻿using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Fetcher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FND.API.Data.Repositories
{
    public class NewsRepository
    {
        private readonly FNDDBContext _fNDDBContext;

        public NewsRepository(FNDDBContext fNDDBContext)
        {
            _fNDDBContext= fNDDBContext;
        }

        public async Task<List<News>> GetNews([FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly)
        {
            var newsList = await _fNDDBContext.News.OrderByDescending(b => b.Id).ToListAsync();
            if (IsFakeNewsOnly)
            {
                newsList = await _fNDDBContext.News.Where(n=>n.Classification_Decision=="Fake").OrderByDescending(i=>i.Id).ToListAsync();
            }
            return newsList;
        }

        public async Task<ActionResult<News>> CreateNews(CreateNewsDto model)
        {
            //await _fNDDBContext.News.AddAsync(createNewsDto);
            //    await _fNDDBContext.SaveChangesAsync();
            News newNews = new News()
            {
                //Id = Guid.NewGuid(),
                Url = model.Url,
                Topic = model.Topic,
                Content = model.Content,
                Publisher_id = model.Publisher_id,
                Classification_Decision = model.Classification_Decision.Replace("\"", ""),
                CreatedOn= DateTime.Now,
            };

            _fNDDBContext.Add<News>(newNews);
            _fNDDBContext.SaveChanges();
            return newNews;

        }

        public async Task<NewsDashboardResult> GetNewsCountByClassification([FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            string format = "yyyy-MM-dd";
            //DateTime fromDateee = DateTime.Parse(fromDate);
            //DateTime toDateee = DateTime.Parse(fromDate);
            
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);
            var newsCountByClassification = await _fNDDBContext.News
                .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
                .GroupBy(n => n.Classification_Decision)
                .Select(g => new NewsClassificationCount { Classification = g.Key, Count = g.Count() })
                .ToListAsync();

            NewsDashboardResult result = new NewsDashboardResult();
            foreach (var classification in newsCountByClassification )
            {
                if (classification.Classification == "Fake")
                {
                    result.fakeCount = classification.Count;
                }
                if (classification.Classification == "Real")
                {
                    result.realCount = classification.Count;
                }
            }
            return result;
        }

        public async Task<List<ReviewRequest>> GetNewsByPublisherId(int publisherId, string filter)
        {

            if (filter == "fakeOnly")
            {
                var newsList = await _fNDDBContext.News
                    .Where(b => b.Publisher_id == publisherId && b.Classification_Decision == "Fake")
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

                List<ReviewRequest> finalNews = new List<ReviewRequest>();
                foreach (var news in newsList)
                {
                    ReviewRequest reviewNews = await _fNDDBContext.ReviewRequest
                        .Where(r => r.Id == news.Id)
                        .Include(rr => rr.News)
                        .SingleOrDefaultAsync();
                    if (reviewNews == null)
                    {
                        reviewNews = new ReviewRequest
                        {
                            News = news
                        };
                    }
                    finalNews.Add(reviewNews);

                }
                return finalNews;
            } else if (filter == "pendingOnly")
            {
                var newsList = await _fNDDBContext.ReviewRequest
                    .Where(r => r.News.Publisher_id == publisherId && r.Status == 0)
                    .OrderByDescending(p => p.Id)
                    .Include(rr => rr.News)
                    .ToListAsync();
                return newsList;
            } else if (filter == "reviewedOnly")
            {
                var newsList = await _fNDDBContext.ReviewRequest
                   .Where(r => r.News.Publisher_id == publisherId && r.Status == 1)
                   .OrderByDescending(p => p.Id)
                   .Include(rr => rr.News)
                   .ToListAsync();
                return newsList;
            } else
            {
                var newsList = await _fNDDBContext.News
                    .Where(b => b.Publisher_id == publisherId)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

                List<ReviewRequest> finalNews = new List<ReviewRequest>();
                foreach (var news in newsList)
                {
                    ReviewRequest reviewNews = await _fNDDBContext.ReviewRequest
                        .Where(r => r.Id == news.Id)
                        .Include(rr => rr.News)
                        .SingleOrDefaultAsync();
                    if (reviewNews == null)
                    {
                        reviewNews = new ReviewRequest
                        {
                            News = news
                        };
                    }
                    finalNews.Add(reviewNews);

                }
                return finalNews;
            }
        }

        public async Task<ReviewRequest> RequestReview(CreateRequestReviewDto createRequestReviewDto)
        {
            ReviewRequest reviewRequest = new ReviewRequest()
            {
                Id = createRequestReviewDto.NewsId,
                Comment = createRequestReviewDto.Comment,
                CreatedOn = DateTime.Now
            };

            _fNDDBContext.AddAsync<ReviewRequest>(reviewRequest);
            _fNDDBContext.SaveChanges();
            return reviewRequest;
        }

        public async Task<ReviewRequest> SubmitReview(int ModeratorId, SubmitReviewDto submitReviewDto)
        {
            //GetReviewRequestedNewsById(submitReviewDto.RequestReviewId);
            ReviewRequest updateReviewRequest = new ReviewRequest()
            {
                Id = submitReviewDto.RequestReviewId,
                ReviewFeedback = submitReviewDto.ReviewFeedback,
                Result = submitReviewDto.ReviewResult,
                Status = 1,
                ReviewedBy = ModeratorId,
                UpdatedOn = DateTime.Now
            };
            _fNDDBContext.Attach(updateReviewRequest);
            _fNDDBContext.Entry(updateReviewRequest).Property(r => r.ReviewFeedback).IsModified = true;
            _fNDDBContext.Entry(updateReviewRequest).Property(r => r.Result).IsModified = true;
            _fNDDBContext.Entry(updateReviewRequest).Property(r => r.Status).IsModified = true;
            _fNDDBContext.Entry(updateReviewRequest).Property(r => r.UpdatedOn).IsModified = true;
            _fNDDBContext.SaveChanges();
            return updateReviewRequest;
        }

        public async Task<List<ReviewRequest>> GetAllReviewRequestedNews(string filter)
        {
            if (filter == "pendingOnly")
            {
                var newsList = await _fNDDBContext.ReviewRequest.Where(r => r.Status == 0)
                                .OrderByDescending(p => p.Id)
                                .Include(rr => rr.News) // Include the related News entity
                                .ToListAsync();
                return newsList;
            } else
            {
                var newsList = await _fNDDBContext.ReviewRequest.Where(r => r.Status == 1)
                                .OrderByDescending(p => p.Id)
                                .Include(rr => rr.News) // Include the related News entity
                                .ToListAsync();
                return newsList;
            }
        }

        //public async Task<List<ReviewRequest>> GetReviewRequestedNewsById(int RequestedReviewId)
        //{
        //    var newsList = await _fNDDBContext.ReviewRequest.Where(r => r.News.Id == RequestedReviewId).OrderByDescending(p => p.Id).ToListAsync();
        //    return newsList;
        //}
        public async Task<List<ReviewRequest>> GetReviewRequestedNewsByPublisher(int userId)
        {
            var newsList = await _fNDDBContext.ReviewRequest.Where(r => r.News.Publisher_id == userId).OrderByDescending(p => p.Id).ToListAsync();
            return newsList;
        }
    }
}

