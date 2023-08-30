using FND.API.Data.Repositories;
using FND.API.Entities;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using FND.API.Data;
using FND.API.Data.Dtos;
using Newtonsoft.Json;

namespace FND.API.Services
{
    public class NewsServiceImpl : NewsService
    {

        private readonly NewsRepository _newsRepository;
        private readonly SubscriberRepository _subscriberRepository;
        private readonly NotificationService _notificationService = new NotificationService();


        public NewsServiceImpl(FNDDBContext fNDDBContext)
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
            string subject = "A new Fake news have been detected";
            string body = "News: <b>"+createNewsDto.Topic+".</b> <br> For more details visit http://localhost:4200/</br>";

            Notification notification = new Notification(subject, subEmail,null, body);

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

        public async Task<List<NewsClassificationCount>> GetNewsCountByClassification([FromQuery(Name = "from")]string fromDate, [FromQuery(Name = "to")] string toDate)
        {
            var newsCountByClassification =  await _newsRepository.GetNewsCountByClassification(fromDate, toDate);
            return newsCountByClassification;
        }

        public async Task<List<ListNewsDto>> GetNewsByPublisherId(int publisherId, string Filter)
        {
            var newsList = await _newsRepository.GetNewsByPublisherId(publisherId, Filter);
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

        public async Task<ReviewRequest> SubmitReview(SubmitReviewDto submitReviewDto)
        {
            var request = await _newsRepository.SubmitReview(submitReviewDto);
            return request;
        }

        public async Task<List<ReviewRequest>> GetAllReviewRequestedNews()
        {
            var request = await _newsRepository.GetAllReviewRequestedNews();
            return request;
        }
    }

    public class ClassificationResutlt
    {
        public string result { get; set; }
    }
}
