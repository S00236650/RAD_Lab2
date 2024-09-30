using Microsoft.EntityFrameworkCore;
using RAD_Lab2_Part5;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AdDb>(opt => opt.UseInMemoryDatabase("AdList"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var Ads = app.MapGroup("/adlist");
var Categories = app.MapGroup("/adlist/categorylist");

Ads.MapGet("/", async (AdDb db) =>
    await db.Ads.ToListAsync());

Ads.MapGet("/categorylist/", async (AdDb db) =>
    await db.Ads.Include(ad => ad.Category).ToListAsync());

Categories.MapPost("/", async (Category category, AdDb db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();

    return Results.Created($"/adlist/categorylist", category);
});

app.Run();