using RabbitMQ.Client;

namespace Memoirist_V2.PostService.RepoPattern.RabbitMQ;

public class PostRabbitRepository : IPostRabbitRepository {
	private readonly ConnectionFactory	_connectionFactory;

	public PostRabbitRepository() {
		_connectionFactory = new ConnectionFactory() {
			HostName = "localhost"
		};
	}
}
