using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FND.API.Data.Repositories
{
    public class UserRepository
    {
        private readonly FNDDBContext fNDDBContext;

        public UserRepository(FNDDBContext fNDDBContext)
        {
            this.fNDDBContext = fNDDBContext;
        }


        public async Task<Moderator> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            //check the email is already exist

            //if not create record with new invite code
            Moderator moderator = new Moderator() {
                Username = createModeratorDto.Email,
                InviteCode = Guid.NewGuid(),
                IsAccepted = false
            };
            await fNDDBContext.Moderators.AddAsync(moderator);
            await fNDDBContext.SaveChangesAsync();
            return moderator;

        }

        public async Task<bool> ValidateModerator(string userName, Guid inviteCode)
        {
            //check the respective email and invitecode is coming 
            bool isvalid = false;
            var dbInviteCode =  fNDDBContext.Moderators.Where(i => i.Username == userName)
                .Select(i => i.InviteCode);
            if (inviteCode == dbInviteCode.FirstOrDefault())
            { isvalid = true; }
            return isvalid;
        }

        public async Task<Moderator> DeleteModerator(string username)
        {
            var result = await fNDDBContext.Moderators
                .FirstOrDefaultAsync(e => e.Username == username);
            if (result != null)
            {
                fNDDBContext.Moderators.Remove(result);
                await fNDDBContext.SaveChangesAsync();
                return result;
            }
            else
            {
                return null;
            }
        }


            private Task<bool> CheckEmailExist(string email)
            => fNDDBContext.Users.AnyAsync(x => x.Email == email);
    }
}
