using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FND.API.Data.Repositories
{
    public class UserRepository
    {
        private readonly FNDDBContext fNDDBContext;

        public UserRepository(FNDDBContext fNDDBContext)
        {
            this.fNDDBContext = fNDDBContext;
        }
        public async Task<Users> CreateUser(SignUpRequestDto signUpRequest)
        {
            signUpRequest.Password = PasswordHasher.HashPassword(signUpRequest.Password);
            Users user = new Users()
            {
                Name = signUpRequest.Name,
                Email = signUpRequest.Email,
                Password_hash = signUpRequest.Password,
                Created_at = DateTime.UtcNow,
                Token = "",
                Role = "Publisher",
                Status = 0
            };
            fNDDBContext.Users.AddAsync(user);
            fNDDBContext.SaveChangesAsync();

            GetUserById(user.Id);

            Publication publication = new Publication()
            {
                Publication_Id = user.Id,
                Publication_Name = signUpRequest.Name,
                RSS_Url = signUpRequest.Url,
                CreatedOn = DateTime.UtcNow
            };
            fNDDBContext.Publications.AddAsync(publication);
            fNDDBContext.SaveChangesAsync();
            return user;

        }
        public async Task<Moderator> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            //check the email is already exist

            //if not create record with new invite code
            Moderator moderator = new Moderator() {
                Username = createModeratorDto.Email,
                InviteCode = Guid.NewGuid()
            };
            await fNDDBContext.Moderators.AddAsync(moderator);
            await fNDDBContext.SaveChangesAsync();
            return moderator;

        }

        public async Task<Moderator> UpdateModerator(int id)
        {
            //check the email is already exist
            var mod = await GetModeratorById(id);
            //if not create record with new invite code
            var updateModerator = new Moderator { Id = id, InviteCode=Guid.NewGuid(), IsUpdated=true, UpdatedAt=DateTimeOffset.UtcNow};
            fNDDBContext.Attach(updateModerator);
            fNDDBContext.Entry(updateModerator).Property(r => r.InviteCode).IsModified = true;
            fNDDBContext.Entry(updateModerator).Property(r => r.IsUpdated).IsModified = true;
            fNDDBContext.Entry(updateModerator).Property(r => r.UpdatedAt).IsModified = true;
            await fNDDBContext.SaveChangesAsync();

            return await GetModeratorById(id);

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

        public async Task<Users> GetUserById(int id)
        {
            var user = await fNDDBContext.Users.Where(u=>u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<Users>> GetModerators(bool isPendingOnly)
        {
            if (isPendingOnly)
            {
                var moderators = await fNDDBContext.Moderators.OrderByDescending(b => b.Id).ToListAsync();
                List<Users> result = new List<Users>();
                foreach (var moderator in moderators)
                {
                    Users users1 = new Users
                    {
                        Id = moderator.Id,
                        Email = moderator.Username,
                        Status = 0
                    };
                    result.Add(users1);
                }
                return result;
            }

            var users = await fNDDBContext.Users.Where(p => p.Role == "Moderator").OrderByDescending(i => i.Id).ToListAsync();
            return users;

        }

        private Task<bool> CheckEmailExist(string email)
            => fNDDBContext.Users.AnyAsync(x => x.Email == email);

        public async Task<Users> GetModeratorUserById(int id)
        {
            Users moderator = await fNDDBContext.Users.Where(u => u.Id == id && u.Role == "Moderator").FirstAsync();
            return moderator;
        }

        public async Task<Moderator> GetModeratorById(int id)
        {
            Moderator moderator = await fNDDBContext.Moderators.Where(u => u.Id==id).FirstAsync();
            return moderator;
        }
        public async Task<bool> DeleteModeratorUser(int id)
        {
            var moderatorUser = GetModeratorUserById(id);

            if (moderatorUser == null)
                return false;

            var deleteModeratorUser = new Users { Id = id, IsDeleted = true, DeletedAt = DateTimeOffset.UtcNow };
            fNDDBContext.Attach(deleteModeratorUser);
            fNDDBContext.Entry(deleteModeratorUser).Property(r => r.IsDeleted).IsModified = true;
            fNDDBContext.Entry(deleteModeratorUser).Property(r => r.DeletedAt).IsModified = true;
            fNDDBContext.SaveChanges();
            return true;
        }
    }
}
