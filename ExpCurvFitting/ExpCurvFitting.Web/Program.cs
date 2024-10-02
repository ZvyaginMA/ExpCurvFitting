using ExpCurvFitting.Web.Components;
using ExpCurvFitting.Application;
using ExpCurvFitting.Core.Models;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .WriteTo.File(new JsonFormatter(), "important.json")
                            .WriteTo.File("all.logs",
                                          restrictedToMinimumLevel: LogEventLevel.Warning,
                                          rollingInterval: RollingInterval.Day)
                            .MinimumLevel.Debug()
                            .CreateLogger();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddTransient<ExcelService>();
builder.Services.AddTransient<ExpModel>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
