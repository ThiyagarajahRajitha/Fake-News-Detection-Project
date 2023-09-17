using FND.API.Data.Repositories;
using FND.API.Entities;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using FND.API.Data;
using FND.API.Data.Dtos;
using Newtonsoft.Json;
using FND.API.Interfaces;

namespace FND.API.Services
{
    public class NewsService : INewsService
    {

        private readonly NewsRepository _newsRepository;
        private readonly SubscriberRepository _subscriberRepository;
        private readonly NotificationService _notificationService = new NotificationService();

        string fakenewsSubject = "A new Fake news have been detected";
        string fakenewsBody = """
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>New Fake News Detected</title>
            </head>
            <body>
                <table width="100%" bgcolor="#f0f0f0">
                    <tr>
                        <td align="center">
                            <table width="600" bgcolor="#ffffff" style="border-radius: 10px; box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);">
                                <tr>
                                    <td style="padding: 20px;">
                                        <h1>New Fake News Detected</h1>
                                        <p>Dear Subscriber,</p>
                                        <p>We are writing to inform you that a new fake news article with the title <b>"{0}"</b> has been detected by our system.</p>
                                        <p>For more details visit http://localhost:4200/</p>
                                        <p>Best regards,</p>
                                        <p>Your Fake News Detection Team</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;


        public NewsService(FNDDBContext fNDDBContext)
        {
            _newsRepository =  new NewsRepository(fNDDBContext);
            _subscriberRepository = new SubscriberRepository(fNDDBContext);
        }

        public async Task<string> ClassifyNews(ClassifyNewsDto classifyNewsDto)//givw the news from user-client
        {
            //var nws = new
            //{
            //    Topic = "Eagle IT Ltd.",
            //    Content = "USA ewjrwtwt wetwktw",
            //    //publisher = "Eagle IT Street 289",
            //    //date = "11-11-2021"
            //};

            //var company = JsonSerializer.Serialize(classifyNewsDto);
            string jsonString = JsonConvert.SerializeObject(classifyNewsDto);
            var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            using var _httpClient = new HttpClient();
            var url = "http://127.0.0.1:8000/news/";
            var response = await _httpClient.PostAsync(url, requestContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            ClassificationResutlt restlJson = JsonConvert.DeserializeObject<ClassificationResutlt>(result);
            string resultValue = restlJson.result;

            CreateNewsDto createNewsDto = new CreateNewsDto()
            {
                Topic = classifyNewsDto.Topic,
                Content = classifyNewsDto.Content,
                Publisher_id = classifyNewsDto.Publication_Id,
                Url = classifyNewsDto.Url,
                Classification_Decision = resultValue
            };
            _newsRepository.CreateNews(createNewsDto);


            IEnumerable<string> subEmail = await _subscriberRepository.GetSubscribersEmail();

            string body = string.Format(fakenewsBody, createNewsDto.Topic);
            Notification notification = new Notification(fakenewsSubject, subEmail,null, body);

            if (resultValue == "Fake")
            {
                _notificationService.SendMailAsync(notification);
            }
            return result;
        }

        public async Task<List<News>> GetNews([FromQuery(Name = "FakeNewsOnly")] bool IsFakeNewsOnly)
        {
            var newsList = await _newsRepository.GetNews(IsFakeNewsOnly);
            return newsList;
        }

        public async Task<NewsDashboardResultDto> GetNewsCountByClassification(int id, [FromQuery(Name = "from")]string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            var newsCountByClassification =  await _newsRepository.GetNewsCountByClassification(id, fromDate, toDate);
            return newsCountByClassification;
        }

        public async Task<ReviewRequestDashboardRestultDto> GetReviewRequestCount(int id,[FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            var reviewREquestCountByStatus = await _newsRepository.GetReviewRequestCount(id,fromDate, toDate);
            return reviewREquestCountByStatus;
        }
        public async Task<List<NewsCountByPublisherDashboardresultDto>> GetNewsClassificationCountByPublisher(int id, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            var newsClassificationCountByPublisher = await _newsRepository.GetNewsClassificationCountByPublisher(id, fromDate, toDate);
            return newsClassificationCountByPublisher;
        }
        public async Task<List<ReviewRequestCountByPublisherDashboardresultDto>> GetReviewRequestCountByPublisher(int id, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            var reviewRequestCountByPublisher = await _newsRepository.GetReviewRequestCountByPublisher(id, fromDate, toDate);
            return reviewRequestCountByPublisher;
        }

        public async Task<List<NewsCountByMonthDashboardresultDto>> GetNewsClassificationCountByMonth(int userId, [FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            var newsClassificationCountByMonth = await _newsRepository.GetNewsClassificationCountByMonth(userId, fromDate, toDate);
            return newsClassificationCountByMonth;
        }

        public async Task<List<ListNewsDto>> GetNewsByPublisherId(int publisherId, string Filter)
        {
            var newsList = await _newsRepository.GetNewsByPublisherId(publisherId, Filter);
            List<ListNewsDto> result = ConvertToListNewsDTO(newsList);
            return result;
        }

        private static List<ListNewsDto> ConvertToListNewsDTO(List<ReviewRequest> newsList)
        {
            List<ListNewsDto> result = new List<ListNewsDto>();
            foreach (var news in newsList)
            {
                ListNewsDto n = new ListNewsDto
                {
                    Id = news.News.Id,
                    Url = news.News.Url,
                    Topic = news.News.Topic,
                    Content = news.News.Content,
                    Publisher_id = news.News.Publisher_id,
                    Classification_Decision = news.News.Classification_Decision,
                    CreatedOn = news.News.CreatedOn,
                    Comment = news.Comment,
                    ReviewCreatedOn = news.CreatedOn,
                    Status = news.Status,
                    ReviewFeedback = news.ReviewFeedback,
                    ReviewedBy = news.Users is null? 0: news.Users.Id,
                    ReviewerName = news.Users is null? "Pending": news.Users.Name,
                    Result = news.Result,
                    UpdatedOn = news.UpdatedOn
                };
                result.Add(n);

            }

            return result;
        }

        public async Task<ReviewRequest> RequestReview(CreateRequestReviewDto createRequestReviewDto)
        {
            var request = await _newsRepository.RequestReview(createRequestReviewDto);
            return request;
        }

        public async Task<ReviewRequest> SubmitReview(int ModeratorId, SubmitReviewDto submitReviewDto)
        {
            var request = await _newsRepository.SubmitReview(ModeratorId,submitReviewDto);
            return request;
        }

        public async Task<List<ListNewsDto>> GetAllReviewRequestedNews(string filter)
        {
            var newsList = await _newsRepository.GetAllReviewRequestedNews(filter);
            List<ListNewsDto> result = ConvertToListNewsDTO(newsList);
            return result;
        }
    }

    public class ClassificationResutlt
    {
        public string result { get; set; }
    }
}
