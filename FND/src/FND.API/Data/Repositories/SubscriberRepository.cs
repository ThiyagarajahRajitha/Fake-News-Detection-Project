using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FND.API.Data.Repositories
{
    public class SubscriberRepository
    {
        private readonly FNDDBContext _fNDDBContext;
        public SubscriberRepository(FNDDBContext fNDDBContext)
        {
            _fNDDBContext = fNDDBContext;
        }

        public async Task<ActionResult<Subscriber>> Subscribe(CreateSubscriberDto model)
        {
            Subscriber newSubscriber = new Subscriber()
            {
                Email = model.Email,
                CreatedOn = DateTime.Now,
            };
            _fNDDBContext.Add<Subscriber>(newSubscriber);
            _fNDDBContext.SaveChanges();
            return newSubscriber;
        }
        public async Task<IEnumerable<string>> GetSubscribersEmail()
        {
            List<Subscriber> subscribersEmail = await _fNDDBContext.Subscribers.ToListAsync();
            List<string> sub = new List<string>();
            foreach (Subscriber subscriber in subscribersEmail)
            {
                sub.Add(subscriber.Email.ToString());
            }
            return sub;
        }
    }
}
