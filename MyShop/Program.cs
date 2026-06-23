using MyShop.MyHelpers;

var builder = WebApplication.CreateBuilder(args);

// For when I was using EF Core, moved to ADO.NET: Register DBContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
//    options.UseSqlServer(connectionString);
//});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(36000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

if (app.Environment.IsDevelopment() && !System.Diagnostics.Debugger.IsAttached)
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        var url = app.Urls.FirstOrDefault(u => u.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            ?? app.Urls.FirstOrDefault()
            ?? "http://localhost:5186";

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch
        {
            // Browser launch is best-effort for local dotnet run.
        }
    });
}

app.Run();
