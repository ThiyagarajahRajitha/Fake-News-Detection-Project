﻿using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FND.API.Services
{
    public class ModeratorService
    {
        private readonly ModeratorRepository _repository;
        private readonly NotificationService _notificationService = new NotificationService();
        private readonly string moderatorInviteSubject = "Invitation to Join Fake News Detection System";
        private readonly string moderatorInviteBody = """
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>Invitation to Join Fake News Detection System</title>
            </head>
            <body style="background-color: #f0f0f0; font-family: Arial, sans-serif;">
                <table width="100%" bgcolor="#f0f0f0">
                    <tr>
                        <td align="center">
                            <table width="600" bgcolor="#ffffff" style="border-radius: 10px; box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);">
                                <tr>
                                    <td style="padding: 20px;">
                                        <h1>Invitation to Join Fake News Detection System</h1>
                                        <p>Dear Moderator,</p>
                                        <p>You have been invited to join the Fake News Detection System as a moderator. Your role will be to manually review news articles for accuracy and credibility.</p>

                                        <p>Click the button below to sign up and start your role as a moderator.</p>
                                        <a href="{0}" style="display: inline-block; text-decoration: none; background-color: #333; color: white; padding: 10px 20px; border-radius: 5px; margin-top: 20px;">Sign Up Now</a>
                                        <br><br>
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
        private FNDDBContext fNDDBContext;

        public ModeratorService(FNDDBContext fNDDBContext)
        {
            _repository = new ModeratorRepository(fNDDBContext);
        }
        //public ModeratorService(FNDDBContext fNDDBContext)
        //{
        //    this.fNDDBContext = fNDDBContext;
        //}

        public async Task<Moderator> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            var moderator = await _repository.CreateModerator(createModeratorDto);
            IEnumerable<string> senders = new string[] { createModeratorDto.Email };
            string body = string.Format(moderatorInviteBody, "http://localhost:4200/moderator-signup?username="
                + moderator.Username + "&inviteCode=" + moderator.InviteCode);
            Notification message = new Notification(moderatorInviteSubject, senders, body);
            await _notificationService.SendMailAsync(message);
            return moderator;
        }

        public async Task<bool> ReInviteModerator(int id)
        {
            Moderator moderator = await _repository.UpdateModerator(id);
            if(!moderator.Username.IsNullOrEmpty())
            {
                IEnumerable<string> senders = new string[] { moderator.Username };
                string body = string.Format(moderatorInviteBody, "http://localhost:4200/moderator-signup?username="
                    + moderator.Username + "&inviteCode=" + moderator.InviteCode);
                Notification message = new Notification(moderatorInviteSubject, senders, body);
                await _notificationService.SendMailAsync(message);
                return true;
            }
            else
            {
                return false;
            }
            
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

        public async Task<bool> DeleteModeratorById(int id)
        {
            bool rslt = await _repository.DeleteModeratorById(id);
            return rslt;
        }

    }
}
