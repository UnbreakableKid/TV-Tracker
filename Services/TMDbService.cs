using TMDbLib.Client;

namespace TEKEVERChallenge.Services;

public class TMDbService : TMDbClient
{
    private readonly IConfiguration _config;

    public TMDbService(IConfiguration config): base(config["TMDB_API_KEY"])
    {
        _config = config;
    }

}