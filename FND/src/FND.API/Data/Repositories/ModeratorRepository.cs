using FND.API.Data.Dtos;
using FND.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FND.API.Data.Repositories
{
    public class ModeratorRepository
    {
        private readonly FNDDBContext fNDDBContext;

        public ModeratorRepository(FNDDBContext fNDDBContext)
        {
            this.fNDDBContext = fNDDBContext;
        }

        public async Task<Moderator> CreateModerator(CreateModeratorDto createModeratorDto)
        {
            //check the email is already exist

            //if not create record with new invite code
            Moderator moderator = new Moderator()
            {
                Username = createModeratorDto.Email,
                InviteCode = Guid.NewGuid()
            };
            await fNDDBContext.Moderators.AddAsync(moderator);
            await fNDDBContext.SaveChangesAsync();
            return moderator;

        }

        public async Task<Moderator> UpdateModerator(int id)
        {
            var moderator = await GetModeratorById(id);

            //if not create record with new invite code
            var updateModerator = new Moderator { Id = id, InviteCode = Guid.NewGuid(), IsUpdated = true, UpdatedAt = DateTimeOffset.UtcNow };
            fNDDBContext.Attach(updateModerator);
            fNDDBContext.Entry(updateModerator).Property(r => r.InviteCode).IsModified = true;
            fNDDBContext.Entry(updateModerator).Property(r => r.IsUpdated).IsModified = true;
            fNDDBContext.Entry(updateModerator).Property(r => r.UpdatedAt).IsModified = true;
            await fNDDBContext.SaveChangesAsync();

            var mod = await GetModeratorById(id);
            return mod;
        }
        public async Task<bool> ValidateModerator(string userName, Guid inviteCode)
        {
            bool isvalid = false;
            var dbInviteCode = fNDDBContext.Moderators.Where(i => i.Username == userName)
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
        public async Task<bool> DeleteModeratorById(int id)
        {
            var result = await fNDDBContext.Moderators
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                fNDDBContext.Moderators.Remove(result);
                await fNDDBContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Moderator> GetModeratorById(int id)
        {
            Moderator moderator = await fNDDBContext.Moderators.Where(u => u.Id == id).FirstAsync();
            return moderator;
        }

    }
}
