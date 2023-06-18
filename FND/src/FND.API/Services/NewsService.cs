using FND.API.Data.Repositories;
using FND.API.Entities;
using System.Text.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FND.API.Data;
using FND.API.Data.Dtos;
using Azure;
using System;
using Newtonsoft.Json;
using System.Net.Mail;

namespace FND.API.Services
{
    public class NewsService
    {

        private readonly NewsRepository _newsRepository;
        private readonly NotificationService _notificationService = new NotificationService();


        public NewsService(FNDDBContext fNDDBContext)
        {
            _newsRepository =  new NewsRepository(fNDDBContext);
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
                //Publisher_id = model.Publisher_id,
                Classification_Decision = resultValue
            };
            _newsRepository.CreateNews(createNewsDto);


            IEnumerable<string> subEmail = await _newsRepository.GetSubscribersEmail();
            string subject = "A new Fake news have been detected";
            string body = "News: <b>"+createNewsDto.Topic+".</b> <br> For more details visit http://localhost:4200/</br>";

            Notification notification = new Notification(subject, subEmail,null, body);

            if (resultValue == "Fake")
            {
                _notificationService.SendMailAsync(notification);
            }
            return result;
        }

        public async Task<List<News>> GetNews()
        {
            var newsList = await _newsRepository.GetNews();
            return newsList;
        }

        public async Task<CreateSubscriberDto> Subscribe(CreateSubscriberDto createSubscriberDto)
        {
            _newsRepository.Subscribe(createSubscriberDto);
            IEnumerable<string> senders = new string[] { createSubscriberDto.Email };
            Notification message = new Notification("Welcome to Fake News detection System", senders, "welcome");
            await _notificationService.SendMailAsync(message);
            return createSubscriberDto ;
        }

    }

    public class ClassificationResutlt
    {
        public string result { get; set; }
    }
}
