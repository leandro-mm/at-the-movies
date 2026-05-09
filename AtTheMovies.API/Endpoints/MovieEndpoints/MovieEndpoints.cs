using AtTheMovies.API.Endpoints.MovieEndpoints;

public static class MovieEndpoints
{   
    public static void MapMovieEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/movies");

        group.MapCreateMovie();
        group.MapUpdateMovie();
        group.MapDeleteMovie();
        group.MapGetMovies();
        group.MapGetMovieById();   
    }
}