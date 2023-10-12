using Xprtz.Training.MicroServices.Api;
using Xprtz.Training.MicroServices.Domain;
using Xprtz.Training.MicroServices.Domain.Interfaces;
using Xprtz.Training.MicroServices.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<ServiceBusService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<ChatServiceConfig>(_ =>
{
    var config = builder.Configuration.GetSection("ChatServiceConfig").Get<ChatServiceConfig>();
    if (config == null)
        throw new Exception("Could not bind service config!");
    
    return config;
});

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

app.Run();