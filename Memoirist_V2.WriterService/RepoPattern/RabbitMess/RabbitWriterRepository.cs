		
using Memoirist_V2.WriterService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Memoirist_V2.WriterService.RepoPattern.RabbitMess;

public class RabbitWriterRepository : IRabbitWriterRepository {
	private readonly ConnectionFactory _factory;
	public RabbitWriterRepository() {
		_factory = new ConnectionFactory() {
			HostName = "localhost"
		};
	}
	public void SendListInt(List<int>? listInt,string QueueName ) { 
		using var connection = _factory.CreateConnection();
		using var channel = connection.CreateModel();
		channel.QueueDeclare(QueueName, false, false, false, null);
		var message = JsonConvert.SerializeObject(listInt);
		var body = Encoding.UTF8.GetBytes(message);
		channel.BasicPublish("",routingKey:QueueName, body:body);
	}

	public void SendMessage(Writer Item, string QueueName) {
		using(var connection = _factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, null);
				var message = JsonConvert.SerializeObject(Item);
				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish(exchange: "", routingKey: QueueName, body: body);
			}
		}
	}
	public async Task<List<Writer>> ReceiveMessage(string QueueName) {
		List<Writer> items = new List<Writer>();
		using(var connection = _factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<bool>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					var writer = JsonConvert.DeserializeObject<Writer>(message);
					Console.WriteLine($"Received mess: {message}");
					items.Add(writer);
				};
				channel.BasicConsume(QueueName, true, consumer);
				await Task.WhenAny(tcs.Task, Task.Delay(1000));
			}
			return items;
		}
	}
	public async Task<Writer> ReceiveMessageObject(string QueueName) {
		Writer item = new Writer();
		using(var connection = _factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<bool>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					var writer = JsonConvert.DeserializeObject<Writer>(message);
					item = writer;
				};
				channel.BasicConsume(QueueName, true, consumer);
				await Task.WhenAny(tcs.Task, Task.Delay(1000));
			}
			return item;
		}
	}
	public async Task<int> ReceiveInt(string QueueName) {
		int item = 0;
		using(var connection = _factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<int>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					var writer = JsonConvert.DeserializeObject<int>(message);
					item = writer;
				};
				channel.BasicConsume(QueueName, true, consumer);
				await Task.WhenAny(tcs.Task, Task.Delay(1000));
			}
			return item;
		}
	}


}
