using RabbitMQ.Client;

namespace RabbitMQ_excelCreate.Service
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMQClientService> _logger;
        public static string ExchangeName = "ExcelDirectExchange";
        public static string RoutingExcel = "excel-file";
        public static string KuyrukName = "kuyruk-excel-file";

        public RabbitMQClientService(ILogger<RabbitMQClientService> logger, ConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;

        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen : true})
                return _channel;

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, type:"direct", true,false);
            _channel.QueueDeclare(KuyrukName, true,false, false, null);
            _channel.QueueBind(exchange: ExchangeName, routingKey: RoutingExcel, queue: KuyrukName);

            _logger.LogInformation("RabbitMq bağlantı kuruldu");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile bağlantı koptu");
        }
    }
}
