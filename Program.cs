using CapstoneTeam11.Services;
using MongoDB.Driver;
using CapstoneTeam11.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// MongoDB connection
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient("mongodb+srv://ekshawhan:vqUud.zGaHSK5a4@ticklr.umbq6.mongodb.net/?retryWrites=true&w=majority&appName=Ticklr"));

// Inject the MongoDB database itself
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("TICKLR"); // replace with your DB name if different
});

// Register services
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<TicketService>();

// Add authentication
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Auth middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // default to login

app.Run();