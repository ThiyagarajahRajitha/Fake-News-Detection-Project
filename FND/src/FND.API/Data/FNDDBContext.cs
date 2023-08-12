using Microsoft.EntityFrameworkCore;

namespace FND.API.Data
{
    public class FNDDBContext : DbContext
    {
        public FNDDBContext(DbContextOptions options) : base(options)
        {

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-NN7S7R94;Database=FND;Encrypt=False;Integrated Security=True");
        //}
        public DbSet<Entities.News> News { get; set; }
        public DbSet<Entities.Subscriber> Subscribers { get; set; }
        public DbSet <Entities.Users> Users { get; set; }
        public DbSet <Entities.Moderator> Moderators { get; set; }


    }
}
