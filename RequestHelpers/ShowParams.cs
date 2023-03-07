namespace TEKEVERChallenge.RequestHelpers
{
    public class ShowParams : PaginationParams
    {
        public string? OrderBy { get; set; }

        public string? SearchTerm { get; set; }

        public string? Genres { get; set; }
    }
}