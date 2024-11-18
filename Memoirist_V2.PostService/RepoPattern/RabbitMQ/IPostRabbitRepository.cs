using Memoirist_V2.PostService.Models;

namespace Memoirist_V2.PostService.RepoPattern.RabbitMQ;

public interface IPostRabbitRepository {
	Task<List<int>> ReceiveListPostIdOfWriter(string QueueName);

}
