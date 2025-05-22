using TechStax.Components;
using TechStax.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// inside builder.Services configuration
builder.Services.AddHttpClient<RemoteOkScraper>(client =>
{
    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "Mozilla/5.0 (compatible; JobBoardAnalyzer/1.0)");
});

builder.Services.AddTransient<IJobBoardScraper, RemoteOkScraper>();

builder.Services.AddHttpClient<MuseScraper>();
builder.Services.AddTransient<IJobBoardScraper, MuseScraper>();

builder.Services.AddSingleton<ITechnologyExtractionService, TechnologyExtractionService>();
builder.Services.AddSingleton<IJobDataService, JobDataService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();