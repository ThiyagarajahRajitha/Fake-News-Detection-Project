using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;

namespace FND.API.Services
{
    public class SubscribeService
    {
        private readonly SubscriberRepository _subscriberRepository;

        private readonly string emailSubject = "Welcome to Fake News detection System";
        private readonly string emailBody = """
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Welcome to Fake News Detection System</title>
            </head>
            <body>
                <table width="100%" bgcolor="#f0f0f0">
                    <tr>
                        <td align="center">
                            <table width="600" bgcolor="#ffffff" style="border-radius: 10px; box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);">
                                <tr>
                                    <td style="padding: 20px;">
                                        <h1>Welcome to Fake News Detection System</h1>
                                        <p>Thank you for subscribing to our Fake News Detection System. You are now part of a community dedicated to combating misinformation.</p>
                                        <p>Stay informed, and help us in our mission to promote accurate news and media integrity.</p>
                                        <p>Best regards,</p>
                                        <p>Your Fake News Detection Team</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;
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
            Notification message = new Notification(emailSubject, senders, emailBody);
            await _notificationService.SendMailAsync(message);
            return createSubscriberDto;
        }
    }
}
