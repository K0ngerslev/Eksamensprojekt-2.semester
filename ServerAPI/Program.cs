using MongoDB.Driver;
using ServerAPI.Chat;
using ServerAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAktivitetRepository, AktivitetRepository>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDeltagerRepository, DeltagerRepository>();
builder.Services.AddScoped<ChatRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

var app = builder.Build();

// Konfigurerer HTTP-request-pipelinen.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("policy");

app.UseAuthorization();

app.MapControllers();

app.Run();
