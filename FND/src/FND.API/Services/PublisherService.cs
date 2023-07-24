using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FND.API.Services
{
    public class PublisherService
    {
        private readonly PublisherRepository _publisherRepository;
        private readonly NotificationService _notificationService = new NotificationService();
        public PublisherService(FNDDBContext fNDDBContext)
        {
            _publisherRepository = new PublisherRepository(fNDDBContext);
        }
        public async Task<List<Users>> GetPublishers([FromQuery(Name = "PendingApprovalOnly")] bool IsPendingOnly)
        {
            var publishersList = await _publisherRepository.GetPublishers(IsPendingOnly);
            return publishersList;
        }

       
        public async Task UpdatePublisherAsync([FromRoute] int id)
        {
            await _publisherRepository.UpdatePublisherAsync(id);

            IEnumerable<string> email = await _publisherRepository.getEmail(id);
            string subject = "Account Activation";
            string body = "Your account has been activated. To Login http://localhost:4200/login";

            Notification notification = new Notification(subject, email, null, body);
            _notificationService.SendMailAsync(notification);
            

        }
    }
}
