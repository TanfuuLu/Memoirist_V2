using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Memoirist_V2.PostService.RepoPattern.RabbitMQ;

public class PostRabbitRepository : IPostRabbitRepository {
	private readonly ConnectionFactory	_connectionFactory;

	public PostRabbitRepository() {
		_connectionFactory = new ConnectionFactory() {
			HostName = "localhost"
		};
	}

	public async Task<List<int>> ReceiveListPostIdOfWriter(string QueueName) {
		List<int> listId = new List<int>();
		using(var connection = _connectionFactory.CreateConnection()) {
			using(var channel = connection.CreateModel()) {
				channel.QueueDeclare(QueueName,false,false,false,null);
				var consumer = new EventingBasicConsumer(channel);
				var tcs = new TaskCompletionSource<List<int>>();
				consumer.Received += (model, ea) => {
					var body = ea.Body.ToArray();
					var message = Encoding.UTF8.GetString(body);
					var postId = JsonConvert.DeserializeObject<int>(message);
					listId.Add(postId);
				};
				channel.BasicConsume(QueueName, true, consumer);
				await Task.WhenAny(tcs.Task, Task.Delay(1000));
			}
			return listId;
		}
	}
}
