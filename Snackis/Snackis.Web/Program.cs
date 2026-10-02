using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Snackis.Core.Entities;
using Snackis.Core.Interfaces;
using Snackis.Infrastructure.Data;
using Snackis.Infrastructure.Repositories;
using Snackis.Infrastructure.Services;
using Snackis.Web.Components;
using Snackis.Web.Components.Account;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authentication
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddScoped<
    AuthenticationStateProvider,
    IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme =
        IdentityConstants.ApplicationScheme;

    options.DefaultSignInScheme =
        IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();

// Database
var connectionString =
    builder.Configuration.GetConnectionString(
        "DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<SnackisDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>));


builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<ITopicService, TopicService>();

builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<ICommentService, CommentService>();

// Identity and roles
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    options.Stores.SchemaVersion =
        IdentitySchemaVersions.Version3;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<SnackisDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();

builder.Services.AddSingleton<
    IEmailSender<AppUser>,
    IdentityNoOpEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

    await RoleSeeder.SeedAsync(roleManager);

    string? adminEmail =
        builder.Configuration["BootstrapAdmin:Email"];

    if (!string.IsNullOrWhiteSpace(adminEmail))
    {
        var userManager =
            scope.ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();

        await AdminUserSeeder.SeedAsync(
            userManager,
            adminEmail.Trim());
    }
}
// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Identity endpoints
app.MapAdditionalIdentityEndpoints();

app.Run();