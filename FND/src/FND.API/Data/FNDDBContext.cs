using Microsoft.EntityFrameworkCore;

namespace FND.API.Data
{
    public class FNDDBContext : DbContext
    {
        public FNDDBContext(DbContextOptions options) : base(options){}
        
        public DbSet<Entities.News> News { get; set; }
        public DbSet<Entities.Subscriber> Subscribers { get; set; }
        public DbSet <Entities.Users> Users { get; set; }
        public DbSet <Entities.Moderator> Moderators { get; set; }
        public DbSet<Entities.Publication> Publications { get; set; }
        public DbSet<Entities.ReviewRequest> ReviewRequest { get; set; }


    }
}
