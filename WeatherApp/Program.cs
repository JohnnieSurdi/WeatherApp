using log4net.Config;
using WeatherApp.Logging;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

//Configure logger
var log4netConfigFilePath = Path.Combine(builder.Environment.ContentRootPath, "Configs", "log4net.config");
XmlConfigurator.Configure(new FileInfo(log4netConfigFilePath));

//Register logger
builder.Services.AddSingleton<IWeatherLogger, Logger>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient to use for API calls
builder.Services.AddHttpClient();

// Add WeatherService for dependency injection
builder.Services.AddScoped<WeatherApp.Api.WeatherApi>();
builder.Services.AddScoped<WeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
