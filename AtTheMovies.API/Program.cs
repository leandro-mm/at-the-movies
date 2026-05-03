using AtTheMovies.API.Extensions;
using AtTheMovies.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddMediatRExtension();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Serve the generated OpenAPI/Swagger JSON
    app.UseSwagger();

    // Serve the Swagger UI
    app.UseSwaggerUI();

}
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapMovieEndpoints();
app.Run();
public partial class Program { }