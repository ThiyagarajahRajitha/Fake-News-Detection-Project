using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;

namespace FND.API.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        private readonly NotificationService _notificationService = new NotificationService();
        public UserService(FNDDBContext fNDDBContext)
        {
            _repository = new UserRepository(fNDDBContext);
        }

        public async Task<Moderator> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            var moderator = await _repository.CreateModerator(createModeratorDto);
            IEnumerable<string> senders = new string[] {createModeratorDto.Email };
            string body = "</b> <br> To Login visit http://localhost:4200/moderator-signup?username=" + moderator.Username+"&inviteCode="+moderator.InviteCode+"</br>";
            Notification message = new Notification("Welcome to Fake News detection System", senders, body);
            await _notificationService.SendMailAsync(message);
            return moderator;
        }

        public async Task<bool> ValidateModerator(string userName, Guid inviteCode)
        {
            bool isvalid = await _repository.ValidateModerator(userName, inviteCode);
            return isvalid;
        }

        public async Task<Moderator> DeleteModerator(string userName)
        {
            var moderator = await _repository.DeleteModerator(userName);
            return moderator;
        }
    }
}
