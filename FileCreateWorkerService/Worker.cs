

using FileWorkerService.Models;

namespace FileCreateWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ClientService _rabbitmqClientService;
        private readonly IServiceProvider _serviceProvider; //Northwind veritabanýný buraya direkt çekemem o yüzden serviceprovider üzerinden aldým
        private IModel _channel;
        public Worker(ILogger<Worker> logger, ClientService rabbitmqClientService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitmqClientService = rabbitmqClientService;
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _channel = _rabbitmqClientService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(ClientService.KuyrukName, false, consumer);
            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }


        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(5000);

            var messageBody = Encoding.UTF8.GetString(@event.Body.ToArray());
            var excel = JsonSerializer.Deserialize<ExcelMessages>(messageBody);

            if (excel == null)
            {
                _logger.LogError("RabbitMQ'dan alýnan mesaj deserialize edilemedi.");
                _channel.BasicNack(@event.DeliveryTag, false, true);
                return;
            }

            using var ms = new MemoryStream();
            var wb = new XLWorkbook();
            var ds = new DataSet();

            ds.Tables.Add(GetTable("products"));
            wb.Worksheets.Add(ds);
            wb.SaveAs(ms);

            MultipartFormDataContent multipartForm = new MultipartFormDataContent();
            multipartForm.Add(new ByteArrayContent(ms.ToArray()), "file", $"{Guid.NewGuid()}.xlsx");

            var baseUrl = "https://localhost:44379/api/files";
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{baseUrl}?fileId={excel.FileId}", multipartForm);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"File (Id: {excel.FileId}) baþarýyla oluþturuldu.");
                _channel.BasicAck(@event.DeliveryTag, false);
            }
            else
            {
                _logger.LogError($"File creation failed for FileId: {excel.FileId}. Response: {response.StatusCode}");
                _channel.BasicNack(@event.DeliveryTag, false, true);
            }
        }


        private DataTable GetTable(string tableName)
        {
            List<Product> products;
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<NorthwindContext>();

                products = context.Products.ToList();
            }
            DataTable dataTable = new DataTable()
            {
                TableName = tableName
            };
            dataTable.Columns.Add("ProductId", typeof(int));
            dataTable.Columns.Add("ProductName", typeof(string));
            dataTable.Columns.Add("UnitPrice", typeof(string));
            dataTable.Columns.Add("QuantityPerUnit", typeof(string)); // bu tablo memory'de oluþuyor

            products.ForEach(x =>
            {
                dataTable.Rows.Add(x.ProductId, x.ProductName, x.UnitPrice, x.QuantityPerUnit);
            });
            return dataTable;


        }
    }
}
