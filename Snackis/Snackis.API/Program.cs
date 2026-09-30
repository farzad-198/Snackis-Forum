using Microsoft.EntityFrameworkCore;
using Snackis.Core.Interfaces;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;
using Snackis.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);


// Database connection

var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<SnackisDbContext>(
    options =>
        options.UseSqlServer(connectionString));


builder.Services.AddScoped(typeof(IRepository<>),typeof(Repository<>));

builder.Services.AddScoped<ICategoryService,CategoryService>();

builder.Services.AddScoped<ITopicService,TopicService>();

builder.Services.AddScoped<IPostService,PostService>();

builder.Services.AddScoped<ICommentService,CommentService>();


// Controllers and OpenAPI

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();


// Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();