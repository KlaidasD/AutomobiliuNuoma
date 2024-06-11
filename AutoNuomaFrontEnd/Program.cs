using AutoNuomaFrontEnd.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<INuomaWebService, NuomaWebService>(_=> new NuomaWebService("http://localhost:5036/"));


var log = new LoggerConfiguration()
.WriteTo.Console()
.WriteTo.File("logs/NuomaFE.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();
Log.Logger = log;
var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
