namespace FileCreateWorkerService.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<RabbitMQClientService> _logger;
        public static string KuyrukName = "kuyruk-excel-file";

        public RabbitMQClientService(ILogger<RabbitMQClientService> logger, ConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;

        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
                return _channel;

            _channel = _connection.CreateModel();
           
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
