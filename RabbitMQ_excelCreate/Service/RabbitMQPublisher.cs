namespace RabbitMQ_excelCreate.Service
{
    public class RabbitMQPublisher
    {
        private readonly RabbitMQClientService _rabbitmqClientService;

        public RabbitMQPublisher(RabbitMQClientService rabbitmqClientService)
        {
            _rabbitmqClientService = rabbitmqClientService;
        }
        public void Publish(CreateExcelMessage excelMessage)
        {
            var channel = _rabbitmqClientService.Connect();
            var bodyString = JsonSerializer.Serialize(excelMessage);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var property = channel.CreateBasicProperties();
            property.Persistent = true;

            channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingExcel, basicProperties: property, body: bodyByte);
        }
    }
}
