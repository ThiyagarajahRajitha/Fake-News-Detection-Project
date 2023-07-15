using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
                //Url = model.Url,
                Topic = model.Topic,
                Content = model.Content,
                //Publisher_id = model.Publisher_id,
                Classification_Decision = model.Classification_Decision.Replace("\"", ""),
                CreatedOn= DateTime.Now,
            };

            _fNDDBContext.Add<News>(newNews);
            _fNDDBContext.SaveChanges();
            return newNews;

        }

        public async Task<List<NewsClassificationCount>> GetNewsCountByClassification([FromQuery(Name = "from")] string fromDate, [FromQuery(Name = "to")] string toDate)
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

            return newsCountByClassification;
        }
    }
}

