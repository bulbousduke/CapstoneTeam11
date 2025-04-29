<<<<<<< HEAD
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
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TicketService>();
builder.Services.AddHttpContextAccessor();

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
=======
using CapstoneTeam11.Services;
using MongoDB.Driver;
using MongoDB.Bson;
using CapstoneTeam11.Models;
var connectionString = Environment.GetEnvironmentVariable("MONGODB_URI");
if (connectionString == null)
{
    Console.WriteLine("You must set your 'MONGODB_URI' environment variable. To learn how to set it, see https://www.mongodb.com/docs/drivers/csharp/current/quick-start/#set-your-connection-string");
    Environment.Exit(0);
}
var client = new MongoClient(connectionString);
// var collection = client.GetDatabase("sample_mflix").GetCollection<BsonDocument>("movies");
// var filter = Builders<BsonDocument>.Filter.Eq("title", "Back to the Future");
// var document = collection.Find(filter).First();
// Console.WriteLine(document);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString)); // register IMongoClient as a singleton so it can be injected
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<TicketService>(); 
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

// register user service interface
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
>>>>>>> origin/main
