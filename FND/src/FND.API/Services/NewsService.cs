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

namespace FND.API.Services
{
    public class NewsService
    {

        private readonly NewsRepository _newsRepository;

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
            var result = await response.Content.ReadAsStringAsync();
            
            CreateNewsDto createNewsDto = new CreateNewsDto()
            {
                Topic = classifyNewsDto.Topic,
                Content = classifyNewsDto.Content,
                //Publisher_id = model.Publisher_id,
                Classification_Decision = result
            };
            _newsRepository.CreateNews(createNewsDto);
            return result;
        }

        public async Task<List<News>> GetNews()
        {
            var newsList = await _newsRepository.GetNews();
            return newsList;
        }

    }
}
