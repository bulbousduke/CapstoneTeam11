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

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
