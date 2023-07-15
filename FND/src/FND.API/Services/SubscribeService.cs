using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;

namespace FND.API.Services
{
    public class SubscribeService
    {
        private readonly SubscriberRepository _subscriberRepository;
        public SubscribeService(FNDDBContext fNDDBContext)
        {
            _subscriberRepository = new SubscriberRepository(fNDDBContext);
        }
        //private readonly NewsRepository _newsRepository;
        private readonly NotificationService _notificationService = new NotificationService();


        public async Task<CreateSubscriberDto> Subscribe(CreateSubscriberDto createSubscriberDto)
        {
            _subscriberRepository.Subscribe(createSubscriberDto);
            IEnumerable<string> senders = new string[] { createSubscriberDto.Email };
            Notification message = new Notification("Welcome to Fake News detection System", senders, "welcome");
            await _notificationService.SendMailAsync(message);
            return createSubscriberDto;
        }
    }
}
