using FileWorkerService.Models;

var builder = Host.CreateApplicationBuilder(args);

// Veritabanı Bağlantısı
builder.Services.AddDbContext<NorthwindContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

//builder.Services.AddRazorRuntimeCompilation();
// RabbitMQ ConnectionFactory
builder.Services.AddSingleton(sp =>
    new ConnectionFactory
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMq")),
        DispatchConsumersAsync = true
    });

// RabbitMQ Client Service
builder.Services.AddSingleton<ClientService>();

// Worker Service
builder.Services.AddHostedService<Worker>();

// Uygulamayı Çalıştır
var host = builder.Build();
host.Run();
