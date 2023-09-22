using FND.API.Controllers;
using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<List<Users>> GetApprovedPublication()
        {
            var publishersList = await _fNDDBContext.Users.Where(u => u.Role == "Publisher").Where(p => p.Status == 1)
               .OrderByDescending(b => b.Id)
               .Include(rr => rr.Publication) // Include the related News entity;
               .ToListAsync();
            return publishersList;
        }

        public async Task<List<Users>> GetPublishers([FromQuery(Name = "PendingApprovalOnly")] bool IsPendingOnly)
        {
            
            if (IsPendingOnly)
            {
                var publishersList = await _fNDDBContext.Users.Where(p => p.Role == "Publisher" && p.Status==0).OrderByDescending(i => i.Id).ToListAsync();
                return publishersList;
            }
            else
            {
                var publishersList = await _fNDDBContext.Users.Where(u => u.Role == "Publisher" && u.IsDeleted==false)
                .OrderByDescending(b => b.Id).ToListAsync();
                return publishersList;
            }
     
        }

        //public async Task UpdatePublisherAsync(int id)
        //{
        //    var updatedPub = new Users { Id = id, Status = 1 };
        //    _fNDDBContext.Attach(updatedPub);
        //    _fNDDBContext.Entry(updatedPub).Property(r => r.Status).IsModified = true;
        //    _fNDDBContext.SaveChanges();
        //}

        public async Task UpdatePublisherAsync(int id, ActivatePublisherDto activatePublisher)
        {
            var updatedPub = new Users { Id = id, Status = 1 };
            _fNDDBContext.Attach(updatedPub);
            _fNDDBContext.Entry(updatedPub).Property(r => r.Status).IsModified = true;

            var updatePublication = new Publication { Publication_Id = id, NewsDiv = activatePublisher.divClass };
            _fNDDBContext.Attach(updatePublication);
            _fNDDBContext.Entry(updatePublication).Property(r => r.NewsDiv).IsModified = true;
            _fNDDBContext.SaveChanges();
        }

        public async Task<bool> RejectPublisher(int id,RejectPublisherDto rejectPublisherDto)
        {
            if (rejectPublisherDto == null)
                return false;

            var rejectPubUser = new Users { Id = id, Status = -1};
            _fNDDBContext.Attach(rejectPubUser);
            _fNDDBContext.Entry(rejectPubUser).Property(r => r.Status).IsModified = true;

            var rejectPublication = new Publication { Publication_Id = id,PublisherRejectReason=rejectPublisherDto.RejectReason };
            _fNDDBContext.Attach(rejectPublication);
            _fNDDBContext.Entry(rejectPublication).Property(r=>r.PublisherRejectReason).IsModified = true;
            _fNDDBContext.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<string>> getEmail(int id)
        {
            List<string> emailList = new List<string>();
            string email = _fNDDBContext.Users.Where(u => u.Id == id).Select(u => u.Email).FirstOrDefault();
            emailList.Add(email);
            return emailList;
        }

        internal async void updateLastFetchedNews(int publication_Id, string newsUrl)
        {
            var updatedPub = new Publication { Publication_Id = publication_Id, LastFetchedNewsUrl = newsUrl };
            _fNDDBContext.Attach(updatedPub);
            _fNDDBContext.Entry(updatedPub).Property(r => r.LastFetchedNewsUrl).IsModified = true;
            _fNDDBContext.SaveChanges();
        }

        public async Task<Publication> getBublisherById(int id)
        {
            
            Publication publisher = _fNDDBContext.Publications.Where(p => p.Publication_Id == id).FirstOrDefault();
            return publisher;
        }

        public async Task<bool> DeletePublisher(int id)
        {
            var publisher = getBublisherById(id);

            if (publisher == null)
                return false;
            
            var deletePub = new Publication { Publication_Id = id, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow };
            var deleteUser = new Users { Id = id, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow };
            _fNDDBContext.Attach(deletePub);
            _fNDDBContext.Entry(deletePub).Property(r => r.IsDeleted).IsModified = true;
            _fNDDBContext.Entry(deletePub).Property(r => r.DeletedAt).IsModified = true;

            _fNDDBContext.Attach(deleteUser);
            _fNDDBContext.Entry(deleteUser).Property(r => r.IsDeleted).IsModified = true;
            _fNDDBContext.Entry(deleteUser).Property(r => r.DeletedAt).IsModified = true;
            _fNDDBContext.SaveChanges();
            return true;
            
        }

        public async Task<List<Publication>> GetPublication()
        {
            var publishersList = await _fNDDBContext.Publications
                .Where(p=>p.IsDeleted==false).ToListAsync();
            return publishersList;
        }
    }
}
