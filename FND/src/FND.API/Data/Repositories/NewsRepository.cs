using FND.API.Data.Dtos;
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
            _fNDDBContext = fNDDBContext;
        }

        public async Task<List<News>> GetNews([FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly)
        {
            var newsList = await _fNDDBContext.News.OrderByDescending(b => b.Id).ToListAsync();
            if (IsFakeNewsOnly)
            {
                newsList = await _fNDDBContext.News.Where(n => n.Classification_Decision == "Fake").OrderByDescending(i => i.Id).ToListAsync();
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
                CreatedOn = DateTime.Now,
            };

            _fNDDBContext.Add<News>(newNews);
            _fNDDBContext.SaveChanges();
            return newNews;

        }

        public async Task<NewsDashboardResultDto> GetNewsCountByClassification([FromRoute] int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            string format = "yyyy-MM-dd";
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

            //DateTime fromDateee = DateTime.Parse(fromDate);
            //DateTime toDateee = DateTime.Parse(fromDate);
            NewsDashboardResultDto result = new NewsDashboardResultDto();
            Users user = await _fNDDBContext.Users.Where(u => u.Id == userId).FirstAsync();
            if (user.Role == "Admin")
            {
                var newsCountByClassification = await _fNDDBContext.News
               .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
               .GroupBy(n => n.Classification_Decision)
               .Select(g => new NewsClassificationCount { Classification = g.Key, Count = g.Count() })
               .ToListAsync();

                foreach (var classification in newsCountByClassification)
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
            }
            else if (user.Role == "Publisher")
            {
                var newsCountByClassification = await _fNDDBContext.News
               .Where(n => n.Publisher_id == userId && n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
               .GroupBy(n => n.Classification_Decision)
               .Select(g => new NewsClassificationCount { Classification = g.Key, Count = g.Count() })
               .ToListAsync();


                foreach (var classification in newsCountByClassification)
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
            }
            return result;
        }
        //public async Task<NewsDashboardResultDto> GetNewsCountByPublisher(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        //{
        //    string format = "yyyy-MM-dd";
        //    DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
        //    DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

        //    //DateTime fromDateee = DateTime.Parse(fromDate);
        //    //DateTime toDateee = DateTime.Parse(fromDate);
        //    NewsDashboardResultDto result = new NewsDashboardResultDto();
        //    Users user = await _fNDDBContext.Users.Where(u => u.Id == userId).FirstAsync();
        //    if (user.Role == "Admin")
        //    {
        //        var newsCountByClassification = await _fNDDBContext.News
        //       .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
        //       .GroupBy(n=>new { n.Publisher_id, n.Classification_Decision })
        //       //.GroupBy(n => n.Classification_Decision)
        //       .Select(g => new NewsClassificationByPublisherCount { PublisherId = g.GroupBy(g=>g.Publisher_id), Classification = g.Key.Classification_Decision, Count = g.Count() })
        //       .ToListAsync();

        //        foreach (var classification in newsCountByClassification.GroupBy(c=>)
        //        {
        //            foreach(var pub in newsCountByClassification.)
        //            if (classification.Classification == "Fake")
        //            {
        //                result.fakeCount = classification.Count;
        //            }
        //            if (classification.Classification == "Real")
        //            {
        //                result.realCount = classification.Count;
        //            }
        //        }
        //    }
        //    return result;

        //}

        public async Task<ReviewRequestDashboardRestultDto> GetReviewRequestCount(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            string format = "yyyy-MM-dd";
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

            //DateTime fromDateee = DateTime.Parse(fromDate);
            //DateTime toDateee = DateTime.Parse(fromDate);
            ReviewRequestDashboardRestultDto result = new ReviewRequestDashboardRestultDto();
            Users user = await _fNDDBContext.Users.Where(u => u.Id == userId).FirstAsync();
            if (user.Role == "Admin")
            {
                var reviewREquestCountByStatus = await _fNDDBContext.ReviewRequest
               .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
               .GroupBy(n => n.Status)
               .Select(g => new ReviewRequestCountDto { Status = g.Key.ToString(), Count = g.Count() })
               .ToListAsync();

                foreach (var reviewRequest in reviewREquestCountByStatus)
                {
                    if (reviewRequest.Status == "0")
                    {
                        result.ReviewRending = reviewRequest.Count;
                    }
                    if (reviewRequest.Status == "1")
                    {
                        result.ReviewCompleted = reviewRequest.Count;
                    }
                }
            }
            else if (user.Role == "Moderator")
            {
                var reviewREquestCountByStatus = await _fNDDBContext.ReviewRequest
               .Where(n => n.ReviewedBy == userId && n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
               .GroupBy(n => n.Status)
               .Select(g => new ReviewRequestCountDto { Status = g.Key.ToString(), Count = g.Count() })
               .ToListAsync();


                foreach (var reviewRequest in reviewREquestCountByStatus)
                {
                    if (reviewRequest.Status == "0")
                    {
                        result.ReviewRending = reviewRequest.Count;
                    }
                    if (reviewRequest.Status == "1")
                    {
                        result.ReviewCompleted = reviewRequest.Count;
                    }
                }
            }

            return result;
        }


        public async Task<List<NewsCountByPublisherDashboardresultDto>> GetNewsClassificationCountByPublisher(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            string format = "yyyy-MM-dd";
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

            //DateTime fromDateee = DateTime.Parse(fromDate);
            //DateTime toDateee = DateTime.Parse(fromDate);
            List<NewsCountByPublisherDashboardresultDto> results = new List<NewsCountByPublisherDashboardresultDto>();
            Users user = await _fNDDBContext.Users.Where(u => u.Id == userId).FirstAsync();
            if (user.Role == "Admin")
            {
                var newsCountByPublisher = await _fNDDBContext.News
               .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
               .GroupBy(n => new { n.Publisher_id, n.Classification_Decision })
               .Select(g => new NewsClassificationByPublisherCount
               {
                   PublisherId = g.Key.Publisher_id ?? 0,
                   Classification = g.Key.Classification_Decision,
                   Count = g.Count()
               })
               .ToListAsync();

                //foreach (var publisher in newsCountByPublisher)
                //{
                //    if (publisher.Classification == "Fake")
                //    {
                //        result.FakeCount = publisher.Count;
                //    }
                //    if (publisher.Classification == "Real")
                //    {
                //        result.RealCount = publisher.Count;
                //    }

                //}

                // Group the results by PublisherId
                var groupedResults = newsCountByPublisher.GroupBy(p => p.PublisherId);

                foreach (var group in groupedResults)
                {
                    var publisherResult = new NewsCountByPublisherDashboardresultDto
                    {
                        PID = group.Key
                    };

                    foreach (var publisher in group)
                    {
                        if (publisher.Classification == "Fake")
                        {
                            publisherResult.FakeCount = publisher.Count;
                        }
                        if (publisher.Classification == "Real")
                        {
                            publisherResult.RealCount = publisher.Count;
                        }
                    }

                    results.Add(publisherResult);
                }
            }

            return results;
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
                        .Include(uu => uu.Users)
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
            else if (filter == "pendingOnly")
            {
                var newsList = await _fNDDBContext.ReviewRequest
                    .Where(r => r.News.Publisher_id == publisherId && r.Status == 0)
                    .OrderByDescending(p => p.Id)
                    .Include(rr => rr.News)
                    .ToListAsync();
                return newsList;
            }
            else if (filter == "reviewedOnly")
            {
                var newsList = await _fNDDBContext.ReviewRequest
                   .Where(r => r.News.Publisher_id == publisherId && r.Status == 1)
                   .OrderByDescending(p => p.Id)
                   .Include(rr => rr.News)
                   .Include(uu => uu.Users)
                   .ToListAsync();
                return newsList;
            }
            else
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
                        .Include(uu => uu.Users)
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
            _fNDDBContext.Entry(updateReviewRequest).Property(r => r.ReviewedBy).IsModified = true;
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
                                .Include(u => u.Users)
                                .ToListAsync();
                return newsList;
            }
            else
            {
                var newsList = await _fNDDBContext.ReviewRequest.Where(r => r.Status == 1)
                                .OrderByDescending(p => p.Id)
                                .Include(rr => rr.News) // Include the related News entity
                                .Include(u => u.Users)
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

        public async Task<List<ReviewRequestCountByPublisherDashboardresultDto>> GetReviewRequestCountByPublisher(int id, string fromDate, string toDate)
        {
            string format = "yyyy-MM-dd";
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

            //DateTime fromDateee = DateTime.Parse(fromDate);
            //DateTime toDateee = DateTime.Parse(fromDate);
            List<ReviewRequestCountByPublisherDashboardresultDto> results = new List<ReviewRequestCountByPublisherDashboardresultDto>();
            Users user = await _fNDDBContext.Users.Where(u => u.Id == id).FirstAsync();
            if (user.Role == "Admin")
            {
                var reviewRequestCountByPublisher = await _fNDDBContext.ReviewRequest
               .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
               .Include(n => n.News)
               .GroupBy(n => new { n.News.Publisher_id, n.Status })
               .Select(g => new ReviewRequestByPublisher
               {
                   PublisherId = g.Key.Publisher_id ?? 0,
                   Status = g.Key.Status,
                   Count = g.Count()
               })
               .ToListAsync();

                // Group the results by PublisherId
                var groupedResults = reviewRequestCountByPublisher.GroupBy(p => p.PublisherId);

                foreach (var group in groupedResults)
                {
                    var publisherResult = new ReviewRequestCountByPublisherDashboardresultDto
                    {
                        PID = group.Key
                    };

                    foreach (var publisher in group)
                    {
                        if (publisher.Status == 0)
                        {
                            publisherResult.ReviewRending = publisher.Count;
                        }
                        if (publisher.Status == 1)
                        {
                            publisherResult.ReviewCompleted = publisher.Count;
                        }
                    }

                    results.Add(publisherResult);
                }
            }

            return results;
        }

        public async Task<List<NewsCountByMonthDashboardresultDto>> GetNewsClassificationCountByMonth(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            string format = "yyyy-MM-dd";
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

            //DateTime fromDateee = DateTime.Parse(fromDate);
            //DateTime toDateee = DateTime.Parse(fromDate);
            List<NewsCountByMonthDashboardresultDto> results = new List<NewsCountByMonthDashboardresultDto>();
            Users user = await _fNDDBContext.Users.Where(u => u.Id == userId).FirstAsync();
            if (user.Role == "Admin")
            {
                var newsCounts = await _fNDDBContext.News
                        .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee)
                        .GroupBy(n => new { n.Classification_Decision, n.CreatedOn.Year, n.CreatedOn.Month })
                        .Select(g => new
                        {
                            Classification = g.Key.Classification_Decision,
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            Count = g.Count()
                        })
                        .ToListAsync();

                var groupedResults = newsCounts
                    .GroupBy(g => new { g.Year, g.Month })
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Month);

                foreach (var group in groupedResults)
                {
                    var result = new NewsCountByMonthDashboardresultDto
                    {
                        Year = group.Key.Year,
                        Month = group.Key.Month,
                    };

                    foreach (var newsCount in group)
                    {
                        if (newsCount.Classification == "Fake")
                        {
                            result.FakeCount = newsCount.Count;
                        }
                        if (newsCount.Classification == "Real")
                        {
                            result.RealCount = newsCount.Count;
                        }
                    }
                    results.Add(result);
                }

               
            }
            return results;
        }
    }
}

