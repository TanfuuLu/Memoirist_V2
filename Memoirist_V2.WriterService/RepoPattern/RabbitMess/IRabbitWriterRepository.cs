using Memoirist_V2.WriterService.Models;

namespace Memoirist_V2.WriterService.RepoPattern.RabbitMess;

public interface IRabbitWriterRepository {
	void SendMessage(Writer Item,string QueueName);
	void SendListFollowingStoryIdOfWriter(List<int>? ListFollowingStoryId, string QueueName);
	void SendListStoryOfWriter(List<int>? ListStoryWriterId, string QueueName);
	void SendListPostOfWriter(List<int>? listPostWriterId, string QueueName);
	Task<List<Writer>> ReceiveMessage(string QueueName);
	Task<Writer> ReceiveMessageObject(string QueueName);
	Task<int> ReceiveInt(string QueueName);

}
