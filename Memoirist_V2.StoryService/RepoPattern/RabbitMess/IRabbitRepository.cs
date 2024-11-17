using Memoirist_V2.StoryService.Models;

namespace Memoirist_V2.StoryService.RepoPattern.RabbitMess;

public interface IRabbitRepository {
	void SendMess(string QueueName, Story item);
	void SendList(string QueueName, List<Story> list);
	void SendInt(string QueueName, int? item);
	Task<List<Story>> ReceiveMess(string QueueName);
	Task<Story> ReceiveObject(string QueueName);
	Task<string> ReceiveString(string QueueName);
	Task<List<int>> ReceiveListFollowingStoryOfWriter(string QueueName);
}
