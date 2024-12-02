namespace RabbitMQ_excelCreate.Service
{
    public class Publisher
    {
        private readonly ClientService _rabbitmqClientService;

        public Publisher(ClientService rabbitmqClientService)
        {
            _rabbitmqClientService = rabbitmqClientService;
        }
        public void Publish(ExcelMessages excelMessage)
        {
            var channel = _rabbitmqClientService.Connect();
            var bodyString = JsonSerializer.Serialize(excelMessage);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var property = channel.CreateBasicProperties();
            property.Persistent = true;

            channel.BasicPublish(exchange: ClientService.ExchangeName, routingKey: ClientService.RoutingExcel, basicProperties: property, body: bodyByte);
        }
    }
}
