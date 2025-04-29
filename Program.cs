using CapstoneTeam11.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using CapstoneTeam11.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration["mongodb+srv://ekshawhan:vqUud.zGaHSK5a4@ticklr.umbq6.mongodb.net/?retryWrites=true&w=majority&appName=Ticklr"]));

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<TicketService>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();