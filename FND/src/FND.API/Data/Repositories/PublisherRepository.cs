using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FND.API.Data.Repositories
{
    public class PublisherRepository
    {
        private readonly FNDDBContext _fNDDBContext;

        public PublisherRepository(FNDDBContext fNDDBContext)
        {
            _fNDDBContext = fNDDBContext;
        }

        public async Task<List<Users>> GetPublishers([FromQuery(Name = "PendingApprovalOnly")] bool IsPendingOnly)
        {
            var publishersList = await _fNDDBContext.Users.Where(u => u.Role == "Publisher").OrderByDescending(b => b.Id).ToListAsync();
            if (IsPendingOnly)
            {
                publishersList = await _fNDDBContext.Users.Where(p =>p.Status==0).OrderByDescending(i => i.Id).ToListAsync();
            }
            return publishersList;
        }

        public async Task UpdatePublisherAsync(int id)
        {
            var updatedPub = new Users { Id = id, Status = 1 };
            _fNDDBContext.Attach(updatedPub);
            _fNDDBContext.Entry(updatedPub).Property(r => r.Status).IsModified = true;
            _fNDDBContext.SaveChanges();
        }

        public async Task<IEnumerable<string>> getEmail(int id)
        {
            List<string> emailList = new List<string>();
            string email = _fNDDBContext.Users.Where(u => u.Id == id).Select(u => u.Email).FirstOrDefault();
            emailList.Add(email);
            return emailList;
        }
    }
}
