using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TEKEVERChallenge.Entities;

namespace TEKEVERChallenge.Data
{
    public class TrackerContext : IdentityDbContext<User, Role, int>
    {
        public TrackerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TvShow> Shows { get; set; }
        public DbSet<TvEpisode> Episodes { get; set; }
        public DbSet<TvSeason> Seasons { get; set; }
        
        public DbSet<Character> Characters { get; set; }
        
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Favorite> Favorites { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().HasData(new Role
            {
                Id = 1,
                Name = "admin",
                NormalizedName = "ADMIN"
            }, new Role
            {
                Id = 2,
                Name = "member",
                NormalizedName = "MEMBER"
            });

        }
    }
}