using Memoirist_V2.YarpGateway.Models;

namespace Memoirist_V2.YarpGateway.RabbitMess;

public interface IRabbitYarpRepository {
	void SendUser(string QueueName, User user);
	
}
