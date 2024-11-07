using BlazorHelloWorld.Components;
using BlazorHelloWorld.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var options = builder.Configuration.GetSection(nameof(OpenTokOptions)).Get<OpenTokOptions>();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<OpenTokOptions>(options);
builder.Services.AddScoped<VideoService>();
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.UseAntiforgery();
app.Run();