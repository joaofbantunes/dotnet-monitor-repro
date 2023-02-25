using Worker;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<BackgroundWorker>();

builder
    .Services
    .AddHttpClient<BackgroundWorker>((s, client) =>
    {
        client.BaseAddress = new Uri(s.GetRequiredService<IConfiguration>().GetValue<string>("ApiBaseUri"));
    });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();