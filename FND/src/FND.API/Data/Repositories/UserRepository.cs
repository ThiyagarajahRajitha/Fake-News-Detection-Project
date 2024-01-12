using FND.API.Data.Dtos;
using FND.API.Entities;
using FND.API.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

            var newUser = GetUserById(user.Id);

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
            var users = await fNDDBContext.Users.Where(p => p.Role == "Moderator" && p.IsDeleted==false).OrderByDescending(i => i.Id).ToListAsync();
            return users;
        }

        private Task<bool> CheckEmailExist(string email)
            => fNDDBContext.Users.AnyAsync(x => x.Email == email);

        public async Task<Users> GetModeratorUserById(int id)
        {
            Users moderator = await fNDDBContext.Users.Where(u => u.Id == id && u.Role == "Moderator").FirstAsync();
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

        public async Task<List<ReviewRequestCountByModeratorDashboardresultDto>> GetReviewRequestCountByModerator(int userId, string fromDate, string toDate)
        {
            string format = "yyyy-MM-dd";
            DateTime fromDatee = DateTime.ParseExact(fromDate, format, CultureInfo.InvariantCulture);
            DateTime toDatee = DateTime.ParseExact(toDate, format, CultureInfo.InvariantCulture);

            List<ReviewRequestCountByModeratorDashboardresultDto> results = new List<ReviewRequestCountByModeratorDashboardresultDto>();
            Users user = await fNDDBContext.Users.Where(u => u.Id == userId).FirstAsync();
            if (user.Role == "Admin" || user.Role == "Moderator")
            {
                var reviewREquestCountByModerator = await fNDDBContext.ReviewRequest
               .Where(n => n.CreatedOn >= fromDatee && n.CreatedOn <= toDatee && n.Status == 1)
               .GroupBy(n => n.ReviewedBy)
               .Select(g => new ReviewRequestCountByModeratorDashboardresultDto
               {
                   MID = (int)g.Key,
                   Count = g.Count()
               })
               .ToListAsync();

                results.AddRange(reviewREquestCountByModerator);
            }
            return results;
        }
    }
}
