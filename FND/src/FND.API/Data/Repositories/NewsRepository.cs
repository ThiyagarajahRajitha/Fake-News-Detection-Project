using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FND.API.Data.Repositories
{
    public class NewsRepository
    {
        private readonly FNDDBContext _fNDDBContext;

        public NewsRepository(FNDDBContext fNDDBContext)
        {
            _fNDDBContext= fNDDBContext;
        }

        public async Task<List<News>> GetNews()
        {
            var newsList = await _fNDDBContext.News.OrderByDescending(b => b.Id).ToListAsync();
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
                Classification_Decision = model.Classification_Decision.Replace("\"", "")
            };

            _fNDDBContext.Add<News>(newNews);
            _fNDDBContext.SaveChanges();
            return newNews;

        }

    }
}

