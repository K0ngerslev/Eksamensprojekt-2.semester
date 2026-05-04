using MongoDB.Driver;
using ServerAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// tilføj mongoDB services nedenunder!!!

/*var connectionString = builder.Configuration.GetConnectionString("MongoDb")
    ?? throw new InvalidOperationException("Connection string 'MongoDb' is not configured.");
var databaseName = builder.Configuration["MongoDb:DatabaseName"]
    ?? throw new InvalidOperationException("MongoDb:DatabaseName is not configured.");

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));
*/
builder.Services.AddScoped<IAktivitet, AktivitetRepository>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("policy");

app.UseAuthorization();

app.MapControllers();

app.Run();
