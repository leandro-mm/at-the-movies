namespace AtTheMovies.DTOs.Movies;

public class MovieResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Genre { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpdate { get; set; }
}