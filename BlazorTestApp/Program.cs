using BlazorTestApp.Data;

var builder = WebApplication.CreateBuilder(args);
var options = builder.Configuration.GetSection(nameof(OpenTokOptions)).Get<OpenTokOptions>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IVideoService>(_ => new VideoService(options));
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();