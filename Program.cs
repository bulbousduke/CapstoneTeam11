using CapstoneTeam11.Services;
using MongoDB.Driver;
using CapstoneTeam11.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
if (connectionString == null)
{
    Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
    Environment.Exit(0);
}

var builder = WebApplication.CreateBuilder(args);

// Get MongoDB URI from Azure App Settings
var mongoUri = builder.Configuration["MONGODB_URI"];
if (string.IsNullOrEmpty(mongoUri))
{
    throw new Exception("❌ MONGODB_URI not found in configuration.");
}

// Register Mongo client and database
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoUri));
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase("TICKLR");
});

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString)); // register IMongoClient as a singleton so it can be injected
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TicketService>(); 
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// MVC and Auth
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// app.UseSession();
// app.MapStaticAssets();

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    // .WithStaticAssets();

app.Run();
 