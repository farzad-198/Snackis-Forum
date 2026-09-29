using Microsoft.EntityFrameworkCore;
using Snackis.Core.Interfaces;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;
using Snackis.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Read the database connection string.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

// Register the database context.
builder.Services.AddDbContext<SnackisDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register repository and forum service.
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

builder.Services.AddScoped<IForumService, ForumService>();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at:
// https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();