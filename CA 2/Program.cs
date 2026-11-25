using CA_2.Data;
using CA_2.Models;
using CA_2.Services;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Add Postgres Database

builder.Services.AddScoped<QuestImportService>();

builder.Services.AddDbContext<EftContext>(opt =>
    opt.UseNpgsql(connectionString));

// Register identity with our endpoints
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<EftContext>();

// Enable Authorisation
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
    using var scope = app.Services.CreateScope();

    var importer = scope.ServiceProvider
        .GetRequiredService<QuestImportService>();

    var path = Path.Combine(
        app.Environment.ContentRootPath,
        "Data",
        "SeedData",
        "quests.json"
    );

    await importer.ImportQuestsAsync(path);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Build the login and reg endpoints (no need for a controller, just let the identity control logins)
app.MapGroup("/api/auth")
    .MapIdentityApi<ApplicationUser>();

app.Run();