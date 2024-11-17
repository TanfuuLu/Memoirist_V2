using Memoirist_V2.YarpGateway.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Memoirist_V2.YarpGateway.RabbitMess;

public class RabbitYarpRepository : IRabbitYarpRepository {
	private readonly ConnectionFactory _factory;

	public RabbitYarpRepository() {
		_factory = new ConnectionFactory() {
			HostName = "localhost"
		};
	}
	public void SendUser(string QueueName, User user) {
		using(var connection = _factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var message = JsonConvert.SerializeObject(user);
				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish("", QueueName, null, body);
			}
		}
	}
}
