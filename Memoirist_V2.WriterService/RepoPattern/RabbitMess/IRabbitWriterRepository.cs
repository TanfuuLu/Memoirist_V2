using Memoirist_V2.WriterService.Models;

namespace Memoirist_V2.WriterService.RepoPattern.RabbitMess;

public interface IRabbitWriterRepository {
	void SendMessage(Writer Item,string QueueName);
	void SendListInt(List<int>? ListInt, string QueueName);
	Task<List<Writer>> ReceiveMessage(string QueueName);
	Task<Writer> ReceiveMessageObject(string QueueName);
	Task<int> ReceiveInt(string QueueName);

}
