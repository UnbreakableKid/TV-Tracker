using Microsoft.AspNetCore.Identity;
using TEKEVERChallenge.Entities;
using TEKEVERChallenge.Extensions;
using TMDbLib.Client;
using TvShow = TEKEVERChallenge.Entities.TvShow;

namespace TEKEVERChallenge.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(TrackerContext context, UserManager<User> userManager, IConfiguration config)
        {
            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    UserName = "bob",
                    Email = "bob@test.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");


                var admin = new User
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "Member", "Admin" });
            }
            // Look for any shows.
            if (context.Shows.Any())
            {
                return;   // DB has been seeded
            }
            
            TMDbClient client = new TMDbClient(config["TMDB_API_KEY"]);

            var bd = client.GetTvShowAsync(1396).Result;
            var BreakigBad  = bd.MapToShow(client);
            BreakigBad.Characters = client.GetTvShowCreditsAsync(1396).Result.Cast.Select(c => c.MapToCharacter(context, BreakigBad)).ToList();
            
            var ct = client.GetTvShowAsync(18347).Result;
            var Community = ct.MapToShow(client);
            Community.Characters = client.GetTvShowCreditsAsync(18347).Result.Cast.Select(c => c.MapToCharacter(context, Community)).ToList();

            var ck = client.GetTvShowAsync(1404).Result;
            var Chuck = ck.MapToShow(client);
            Chuck.Characters = client.GetTvShowCreditsAsync(1404).Result.Cast.Select(c => c.MapToCharacter(context, Chuck)).ToList();
            
            
            var shows = new List<TvShow>()
            {
                BreakigBad,
                Community,
                Chuck
            };
            
            context.Episodes.AddRange(shows.SelectMany(s => s.Seasons.SelectMany(ss => ss.TvEpisodes)));
            context.Seasons.AddRange(shows.SelectMany(s => s.Seasons));
            context.Shows.AddRange(shows);

            await context.SaveChangesAsync();

        }
    }
}