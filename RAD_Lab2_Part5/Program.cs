using Microsoft.EntityFrameworkCore;
using RAD_Lab2_Part5;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AdDb>(opt => opt.UseInMemoryDatabase("AdList"));
var app = builder.Build();

var Ads = app.MapGroup("/adlist");
var Categories = app.MapGroup("/adlist/categorylist");
var Sellers = app.MapGroup("/adlist/sellerlist");

Ads.MapGet("/", async (AdDb db) =>
    await db.Ads.Include(ad => ad.Category).Include(ad => ad.Seller).ToListAsync());

Categories.MapGet("/", async (AdDb db) =>
    await db.Categories.Include(c => c.ads).ToListAsync());

Categories.MapPost("/", async (Category category, AdDb db) =>
{
    db.Categories.Add(category);
    await db.SaveChangesAsync();
    return Results.Created($"/categorylist/{category.CategoryId}", category);
});

// Seller endpoints
Sellers.MapGet("/", async (AdDb db) =>
    await db.Sellers.ToListAsync());

Sellers.MapPost("/", async (Seller seller, AdDb db) =>
{
    db.Sellers.Add(seller);
    await db.SaveChangesAsync();
    return Results.Created($"/sellerlist/{seller.SellerId}", seller);
});

Ads.MapPost("/", async (Ad newAd, AdDb db) =>
{
    // Validate the Description
    if (string.IsNullOrWhiteSpace(newAd.Description))
    {
        return Results.BadRequest("Description cannot be empty.");
    }

    // Find the seller and category by ID
    var seller = await db.Sellers.FindAsync(newAd.SellerId);
    var category = await db.Categories.Include(c => c.ads).FirstOrDefaultAsync(c => c.CategoryId == newAd.CategoryId);

    if (seller == null)
    {
        return Results.NotFound($"Seller with ID {newAd.SellerId} not found.");
    }

    if (category == null)
    {
        return Results.NotFound($"Category with ID {newAd.CategoryId} not found.");
    }

    // Associate the new ad with the seller and category
    newAd.Seller = seller;
    newAd.Category = category;

    // Add the ad to the category's list of ads
    category.ads.Add(newAd);
    db.Ads.Add(newAd);
    await db.SaveChangesAsync();

    return Results.Created($"/adlist/{newAd.Id}", newAd);
});


app.Run();