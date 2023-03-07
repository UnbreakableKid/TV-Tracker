using Microsoft.AspNetCore.Identity;

namespace TEKEVERChallenge.Entities
{
    public class User : IdentityUser<int>
    {
        //favorite tv shows
        public virtual ICollection<TvShow> Favorites { get; set; }
    }
}