using AutomobiliuNuoma.Contracts;
using AutomobiliuNuoma.Repositories;
using AutomobiliuNuoma.Repository;
using AutomobiliuNuoma.Services;
using MongoDB.Driver;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>(_=> new DatabaseRepository("Server=DESKTOP-9849SKM;Database=autonuoma;Integrated Security=True;"));
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddSingleton<IMongoRepository, MongoRepository>(sp => new MongoRepository(sp.GetRequiredService<IMongoClient>()));

builder.Services.AddSingleton<INuomaService, NuomaService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var nuomaService = app.Services.GetRequiredService<INuomaService>();
_ = nuomaService.RunCleanupJob();

app.Run();
