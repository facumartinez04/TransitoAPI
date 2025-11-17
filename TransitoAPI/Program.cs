using Microsoft.Extensions.Options;
using TransitoAPI.Rabbit;
using TransitoAPI.Services.Implementations;
using TransitoAPI.Services.Interfaces;
using TransitoAPI.Services.Simulacion;
using TransitoAPI.ZeroMQ;
using TransitoAPI.ZeroMQ.Consumer;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowDashboard", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();


builder.Services.AddSingleton<SimuladorService>();
builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddHostedService<TrafficConsumer>();
builder.Services.AddSingleton<EventQueueService>();
builder.Services.AddHostedService<QueueDispatcherService>();


var apiUrl = builder.Configuration["ApiExterna:BaseUrl"];

builder.Services.AddHttpClient<ITollService, TollService>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

builder.Services.AddSingleton<SimuladorService>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowDashboard");

app.UseStaticFiles();


app.MapHub<TrafficHub>("/trafficHub");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
