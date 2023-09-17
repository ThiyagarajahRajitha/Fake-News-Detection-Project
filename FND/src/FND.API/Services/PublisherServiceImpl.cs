using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace FND.API.Services
{
    public class PublisherServiceImpl : PublisherService
    {
        private readonly PublisherRepository _publisherRepository;
        private readonly NotificationService _notificationService = new NotificationService();
        private readonly string subject = "Welcome to Fake News Detection System";
        private readonly string publisherInviteBody = """
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Welcome to Fake News Detection System</title>
            </head>
            	<body style="background-color: #f0f0f0; font-family: Arial, sans-serif;">	
                <table width="100%" bgcolor="#f0f0f0">	
                    <tr>	
                        <td align="center">	
                            <table width="600" bgcolor="#ffffff" style="border-radius: 10px; box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);">
                                <tr>
                                    <td style="padding: 20px;">
                                        <h1>Welcome to Fake News Detection System</h1>
                                        <p>Dear Publisher,</p>
                                        <p>Your signup request for the Fake News Detection System has been approved by the admin. You can now access your publisher account to submit news articles for analysis.</p>
                                        <p>Click the button below to log in and get started:</p>
                                        <a href="http://localhost:4200/login" style="display: inline-block; text-decoration: none; background-color: #333; color: white; padding: 10px 20px; border-radius: 5px; margin-top: 20px;">Log In Now</a>
                                        <p>We are excited to have you as part of our platform and look forward to your contributions!</p>
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

        public PublisherServiceImpl(FNDDBContext fNDDBContext)
        {
            _publisherRepository = new PublisherRepository(fNDDBContext);
        }

        public async Task<List<Users>> GetApprovedPublication()
        {
            var publishersList = await _publisherRepository.GetApprovedPublication();
            return publishersList;
        }

        public async Task<List<Users>> GetPublishers([FromQuery(Name = "PendingApprovalOnly")] bool IsPendingOnly)
        {
            var publishersList = await _publisherRepository.GetPublishers(IsPendingOnly);
            return publishersList;
        }


        public async Task UpdatePublisherAsync([FromRoute] int id, [FromBody] ActivatePublisherDto activatePublisher)
        {
            await _publisherRepository.UpdatePublisherAsync(id, activatePublisher);

            IEnumerable<string> email = await _publisherRepository.getEmail(id);
            Notification notification = new Notification(subject, email, null, publisherInviteBody);
            _notificationService.SendMailAsync(notification);
        }

        public async Task<bool> RejectPublisher(int id, RejectPublisherDto rejectPublisherDto)
        {
            bool rslt = await _publisherRepository.RejectPublisher(id, rejectPublisherDto);
            return rslt;
        }

        public async void updateLastFetchedNews(int publication_Id, string newsUrl)
        {
            _publisherRepository.updateLastFetchedNews(publication_Id, newsUrl);
        }

        public async Task<bool> DeletePublisher(int id)
        {
            bool rslt= await _publisherRepository.DeletePublisher(id);
            return rslt;
        }

        public async Task<List<Publication>> GetPublication()
        {
            var publishersList = await _publisherRepository.GetPublication();
            return publishersList;
        }
    }
}
