using TEKEVERChallenge.Entities;

namespace TEKEVERChallenge.Extensions
{
    public static class ActorExtensions
    {
        public static IQueryable<Actor> Sort(this IQueryable<Actor> query, string? orderBy)
        {
            query = orderBy switch
            {
                "name" => query.OrderBy(p => p.Name),
                "gender" => query.OrderBy(p => p.Gender),
                _ => query.OrderBy(p => p.Name)

            };

            return query;
        }

        public static IQueryable<Actor> Search(this IQueryable<Actor> query, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }
            var lowerCased = searchTerm.Trim().ToLowerInvariant();

            return query.Where(p => p.Name.ToLower().Contains(lowerCased));
        }
        

    }
}