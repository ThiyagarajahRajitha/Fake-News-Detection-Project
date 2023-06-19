using FND.API.Entities;
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
