var builder = Host.CreateApplicationBuilder(args);

// Veritabanı Bağlantısı
builder.Services.AddDbContext<AdventureWorksLt2022Context>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

// RabbitMQ ConnectionFactory
builder.Services.AddSingleton(sp =>
    new ConnectionFactory
    {
        Uri = new Uri(builder.Configuration.GetConnectionString("RabbitMq")),
        DispatchConsumersAsync = true
    });

// RabbitMQ Client Service
builder.Services.AddSingleton<RabbitMQClientService>();

// Worker Service
builder.Services.AddHostedService<Worker>();

// Uygulamayı Çalıştır
var host = builder.Build();
host.Run();
