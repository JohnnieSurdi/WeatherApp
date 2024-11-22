using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using log4net.Config;
using WeatherApp.Logging;
using WeatherApp.Repositories;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICacheService, CacheService>();

//Configure logger
var log4netConfigFilePath = Path.Combine(builder.Environment.ContentRootPath, "Configs", "log4net.config");
XmlConfigurator.Configure(new FileInfo(log4netConfigFilePath));
builder.Services.AddSingleton<IWeatherLogger, Logger>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

// Register AWS services
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

builder.Services.AddScoped<WeatherApp.Api.WeatherApi>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IWeatherSearchRepository, WeatherSearchRepository>();

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
