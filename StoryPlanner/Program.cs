using Microsoft.EntityFrameworkCore;
using StoryPlanner;
using StoryPlanner.Components;
using StoryPlanner.Services;

var builder = WebApplication.CreateBuilder(args);

//holds the current file name
builder.Services.AddSingleton<StorySession>();

// 2. Register the DbContext Factory to use the Session's path
builder.Services.AddDbContextFactory<AppDbContext>((serviceProvider, options) =>
{
    var session = serviceProvider.GetRequiredService<StorySession>();
    options.UseSqlite(session.GetConnectionString());
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // This command creates the DB if it doesn't exist, 
    // AND applies any pending migrations (updates) if it does.
    dbContext.Database.Migrate(); 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();