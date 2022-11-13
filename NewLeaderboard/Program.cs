using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewLeaderboard.Models;
using NewLeaderboard.Data;
/*
using System.Net.Http.Headers;

using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(
    new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

await ProcessRepositoriesAsync(client);

static async Task ProcessRepositoriesAsync(HttpClient client)
{
    var json = await client.GetStringAsync(
         "https://jsonplaceholder.typicode.com/todos");

    Console.Write(json);
}
*/

// Make a call to web API
/*NewLeaderboard.HttpClientObj clientObj = new NewLeaderboard.HttpClientObj();
await clientObj.API();*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<NewLeaderboardContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NewLeaderboardContext") ?? throw new InvalidOperationException("Connection string 'NewLeaderboardContext' not found.")));

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
