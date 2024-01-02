using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

namespace TradingPOC.TradingAPI.ServiceBusExtention {
	public class RabbitMQPublisher {
		private readonly ConnectionFactory _connectionFactory;
		private readonly string _queueName = "orderQueue";

		public RabbitMQPublisher(ConnectionFactory connectionFactory) {
			_connectionFactory = connectionFactory;
		}

		public void PublishOrder(string orderData) {
			try {
				using var connection = _connectionFactory.CreateConnection();
				using var channel = connection.CreateModel();
				channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

				var body = System.Text.Encoding.UTF8.GetBytes(orderData);

				channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
			} catch (Exception ex) {
				Console.WriteLine($"Error publishing order: {ex.Message}");
			}
		}
	}
}
