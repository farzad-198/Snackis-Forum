using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Snackis.Core.Interfaces;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;
using Snackis.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Database connection
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<SnackisDbContext>(
    options => options.UseSqlServer(connectionString));

// Repository and services
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// API documentation for development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference("/api", options =>
    {
        options.WithTitle("Snackis API");
        options.DisableAgent();
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();