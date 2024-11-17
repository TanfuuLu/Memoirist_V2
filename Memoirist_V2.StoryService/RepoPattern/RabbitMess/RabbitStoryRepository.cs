using Memoirist_V2.StoryService.Models;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Memoirist_V2.StoryService.RepoPattern.RabbitMess;

public class RabbitStoryRepository : IRabbitRepository {
	private readonly ConnectionFactory factory;

	public RabbitStoryRepository() {
		factory = new ConnectionFactory() { HostName = "localhost" };

	}

	public async Task<List<Story>> ReceiveMess(string QueueName) {
		var listItem = new List<Story>();
		using(var connection = factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<List<Story>>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					listItem.Add(JsonConvert.DeserializeObject<Story>(message));
				};
				channel.BasicConsume(queue:QueueName, true, consumer:consumer);
				await Task.WhenAny(tcs.Task, Task.Delay(500));
			}
			return listItem;
		}
	}
	public async Task<string> ReceiveString(string QueueName) {
		var result = "";
		using var connection = factory.CreateConnection();
		using var channel = connection.CreateModel();
		channel.QueueDeclare(QueueName, false, false, false, null);
		var consumer = new EventingBasicConsumer(channel);
		var tcs = new TaskCompletionSource<string>();
		consumer.Received += async (model, ea) => {
			var body = ea.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);
			 result = JsonConvert.DeserializeObject<string>(message);
			await Task.WhenAny(tcs.Task, Task.Delay(500));
		};
		return result;
	}

	public async Task<Story> ReceiveObject(string QueueName) {
		var storyObj = new Story();
		using(var connection = factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<Story>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					 storyObj = JsonConvert.DeserializeObject<Story>(message);
				
				};
				channel.BasicConsume(QueueName, true, consumer);
				await Task.WhenAny(tcs.Task,Task.Delay(500));
			}
			return storyObj;
		}
	}
	public async Task<List<int>> ReceiveListFollowingStoryOfWriter(string QueueName) {
		var listItem = new List<int>();
		using(var connection = factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<List<int>>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					var item = JsonConvert.DeserializeObject<List<int>>(message);
					listItem = item;
				};
				channel.BasicConsume(queue: QueueName, true, consumer: consumer);
				await Task.WhenAny(tcs.Task, Task.Delay(500));
			}
			return listItem;
		}
	}

	public void SendList(string QueueName, List<Story> list) {
		using(var connection = factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var message = JsonConvert.SerializeObject(list);
				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish("", QueueName, null, body);
			}
		}
	}

	public void SendMess(string QueueName, Story item) {
		using(var connection = factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var message = JsonConvert.SerializeObject(item);
				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish("", QueueName,null, body);
			}
		}
	}

	public void SendInt(string QueueName, int? item) {
		using(var connection = factory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName, false, false, false, null);
				var message = JsonConvert.SerializeObject(item);
				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish("", QueueName, null, body);
			}
		}
	}

}
