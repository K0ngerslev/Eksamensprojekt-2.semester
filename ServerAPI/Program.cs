using MongoDB.Driver;
using ServerAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAktivitetRepository, AktivitetRepository>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDeltagerRepository, DeltagerRepository>();
builder.Services.AddScoped<ITeamchatRepository, TeamchatRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("policy");

app.UseStaticFiles();

app.MapControllers();

// Manuelt fallback til index.html for Blazor routing
app.MapFallbackToFile("index.html");

app.Run();