namespace TEKEVERChallenge.RequestHelpers
{
    public class ActorParams : PaginationParams
    {
        public string? OrderBy { get; set; }

        public string? SearchTerm { get; set; }
        
    }
}